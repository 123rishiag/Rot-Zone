using Game.Controls;
using Game.Event;
using Game.Utility;
using Game.Vision;
using Game.Weapon;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Player
{
    public class PlayerController : IStateOwner<PlayerController>
    {
        // Private Variables
        private PlayerModel playerModel;
        private PlayerView playerView;
        private PlayerAnimationController playerAnimationController;
        private PlayerWeaponController playerWeaponController;

        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine playerMovementStateMachine;
        private PlayerActionStateMachine playerActionStateMachine;

        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        private float verticalVelocity;
        private float currentSpeed;

        private Vector3 inputDirection;

        private bool isLocking;

        private RaycastHit lastHit;
        private Vector3 hitPoint;

        private Vector2 aimPosition;

        private int currentHealth;
        public bool IsAlive { get; set; }

        private bool isRecentlyAttacked;

        // Private Services
        public EventService EventService { get; private set; }
        public InputService InputService { get; private set; }
        private CameraService cameraService;

        public PlayerController(PlayerData _playerData, PlayerView _playerPrefab, Vector3 _spawnPosition,
            EventService _eventService, InputService _inputService, WeaponService _weaponService, CameraService _cameraService)
        {
            // Setting Variables
            playerModel = new PlayerModel(_playerData);
            playerView = Object.Instantiate(_playerPrefab).GetComponent<PlayerView>();
            playerView.Init(this);
            playerAnimationController = new PlayerAnimationController(playerView.GetAnimator(), this);
            playerWeaponController = new PlayerWeaponController(this, _eventService, _weaponService);

            // Setting Services
            EventService = _eventService;
            InputService = _inputService;
            cameraService = _cameraService;

            // Setting Elements
            CreateStateMachine();
            Reset(_playerData, _spawnPosition);
        }

        private void CreateStateMachine()
        {
            Owner = this;
            playerMovementStateMachine = new PlayerMovementStateMachine(this);
            playerActionStateMachine = new PlayerActionStateMachine(this);
        }

        public void Reset(PlayerData _playerData, Vector3 _spawnPosition)
        {
            // Setting Variables
            playerMovementStateMachine.ChangeState(PlayerMovementState.IDLE);
            playerActionStateMachine.ChangeState(PlayerActionState.NONE);

            playerModel.Reset(_playerData);

            lastMoveDirection = Vector3.zero;
            verticalVelocity = 0f;
            currentSpeed = 0f;

            isLocking = false;

            aimPosition = Vector2.zero;

            currentHealth = 0;
            IsAlive = true;

            isRecentlyAttacked = false;

            playerView.SetPosition(_spawnPosition);
            playerView.SetRagDollActive(true);

            AssignInputs();
        }

        public void Update()
        {
            playerMovementStateMachine.Update();
            playerActionStateMachine.Update();

            playerAnimationController.UpdateAnimation();
        }

        #region UI
        public void UpdateUI()
        {
            UpdateHealthUI();
            UpdateAmmoUI();
        }
        private void UpdateHealthUI()
        {
            EventService.OnPlayerHealthUIUpdateEvent.Invoke(currentHealth);
        }
        public void UpdateAmmoUI()
        {
            if (playerWeaponController.GetCurrentWeaponType() != WeaponType.NONE)
            {
                WeaponController weaponController = playerWeaponController.GetCurrentWeapon();
                EventService.OnPlayerAmmoUIUpdateEvent.Invoke(weaponController.CurrentAmmo, weaponController.TotalAmmoLeft);
            }
            else
            {
                EventService.OnPlayerAmmoUIUpdateEvent.Invoke(0, 0);
            }
        }
        #endregion

        #region Input
        private void AssignInputs()
        {
            // Not taking inputs if Player is Falling
            if (playerMovementStateMachine.GetCurrentState() == PlayerMovementState.FALL)
                return;

            InputControls inputControls = InputService.GetInputControls();

            inputControls.Player.Movement.performed += ctx =>
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                inputDirection = new Vector3(input.x, 0f, input.y);
            };
            inputControls.Player.Movement.canceled += ctx => inputDirection = Vector3.zero;

            inputControls.Player.Run.performed += ctx => IsRunning = true;
            inputControls.Player.Run.canceled += ctx => IsRunning = false;

            inputControls.Player.Run.performed += ctx => IsRunning = true;
            inputControls.Player.Run.canceled += ctx => IsRunning = false;

            inputControls.Player.Fire.performed += ctx => IsFiring = true;
            inputControls.Player.Fire.canceled += ctx => IsFiring = false;

            inputControls.Player.Lock.performed += ctx => isLocking = true;
            inputControls.Player.Lock.canceled += ctx => isLocking = false;

            inputControls.Player.WeaponOne.started += ctx => playerWeaponController.EquipWeapon(WeaponType.PISTOL);
            inputControls.Player.WeaponTwo.started += ctx => playerWeaponController.EquipWeapon(WeaponType.RIFLE);
            inputControls.Player.WeaponThree.started += ctx => playerWeaponController.EquipWeapon(WeaponType.SHOTGUN);
            inputControls.Player.WeaponStow.started += ctx => playerWeaponController.EquipWeapon(WeaponType.NONE);
            inputControls.Player.WeaponReload.started += ctx => IsReloading = true;

            inputControls.Player.MousePosition.performed += ctx => aimPosition = ctx.ReadValue<Vector2>();
            inputControls.Player.MousePosition.canceled += ctx => aimPosition = Vector2.zero;
        }
        #endregion

        #region Player Movement
        public void UpdateMovementVariables()
        {
            UpdateDirection();
            RotatePlayer();
            ApplyGravity();
            UpdateSpeed();
        }
        public void MovePlayer()
        {
            playerView.GetCharacterController().Move(moveDirection * currentSpeed * Time.deltaTime);
        }
        private void UpdateDirection()
        {
            // Fetching target direction based on below conditions
            Vector3 targetDirection = Vector3.zero;
            // If Player is pressing any movement input, movement direction will be based on movement input
            if (inputDirection.magnitude > 0.1f)
            {
                // Fetching Target Direction where player is trying to move in world based on input and camera 
                targetDirection = (GetXZNormalized(cameraService.GetCurrentCameraTransform().forward) * inputDirection.z +
                    GetXZNormalized(cameraService.GetCurrentCameraTransform().right) * inputDirection.x).normalized;

                lastMoveDirection = moveDirection;
            }
            else if (currentSpeed > 0.1f)
            {
                targetDirection = lastMoveDirection;
            }

            //  Smoothly changing move Direction towards target Direction
            moveDirection = Vector3.Lerp(moveDirection, targetDirection,
                Time.deltaTime * playerModel.DirectionSmoothSpeed);
        }
        private void RotatePlayer()
        {
            // Rotate Player Towards Camera, when player is not falling
            if (playerMovementStateMachine.GetCurrentState() == PlayerMovementState.FALL)
                return;

            // Direction from player to hit point
            Vector3 targetLocation = (hitPoint - playerView.transform.position);
            targetLocation.y = 0f;
            targetLocation.Normalize();

            // Current Player forward
            Vector3 playerForward = playerView.transform.forward;
            playerForward.y = 0f;
            playerForward.Normalize();

            float angle = Vector3.Angle(playerForward, targetLocation);
            if (angle < 25f)
            {
                return;
            }
            Quaternion targetRotation = Quaternion.LookRotation(targetLocation);
            playerView.transform.rotation = Quaternion.RotateTowards(
                playerView.transform.rotation, targetRotation, Time.deltaTime * playerModel.RotationSpeed * 10f);
        }
        private void ApplyGravity()
        {
            // If Player is on Ground, give some velocity to keep the player grounded,
            // else reduce velocity by gravity Scale Factor
            if (IsGrounded() && verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity -= playerModel.GravityFactor * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;
        }
        private void UpdateSpeed()
        {
            // Fetching target speed based on inputs
            float targetSpeed = inputDirection != Vector3.zero ?
                (IsRunning ? playerModel.RunSpeed : playerModel.WalkSpeed) : 0f;

            // Applying Acceleration and DeAcceleration
            if (currentSpeed < targetSpeed)
            {
                currentSpeed += playerModel.AccelerationFactor * Time.deltaTime;

                // Clamping Current Speed to Target Speed, should not go above target speed while accelerating
                if (currentSpeed > targetSpeed)
                    currentSpeed = targetSpeed;
            }
            else if (currentSpeed > targetSpeed)
            {
                currentSpeed -= playerModel.DecelerationFactor * Time.deltaTime;

                // Clamping Current Speed to Target Speed, should not go below target speed while deaccelerating
                if (currentSpeed < targetSpeed)
                    currentSpeed = targetSpeed;
            }
        }
        #endregion

        #region Player Action
        public void UpdateActionVariables()
        {
            AimPlayer();
        }
        private void AimPlayer()
        {
            Vector3 newHitPoint;
            Vector3 newOffset;
            if (!TryUseLockedTarget(out newHitPoint, out newOffset))
            {
                // Setting Aim Based on Mouse Position & Clamping aim at screen bounds
                Vector2 clampedAimPosition = ClampToCenterOffset(aimPosition);

                Ray ray = Camera.main.ScreenPointToRay(clampedAimPosition);
                int combinedLayerMask = playerModel.AimLayer | playerModel.LockLayer;

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, combinedLayerMask))
                {
                    newHitPoint = hit.point;
                    newOffset = hit.normal;
                    lastHit = hit;
                }
                else
                {
                    newHitPoint = ray.GetPoint(100f);
                    newOffset = ray.direction;
                }
            }

            hitPoint = Vector3.Lerp(hitPoint, newHitPoint, Time.deltaTime * playerModel.RotationSpeed);

            UpdateAim();

            playerView.DrawDebugCircle(hitPoint, lastHit.normal, 1f);
        }
        private Vector2 ClampToCenterOffset(Vector2 pos)
        {
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 offset = pos - center;

            float maxX = Screen.width * 0.35f;
            float maxY = Screen.height * 0.35f;

            offset.x = Mathf.Clamp(offset.x, -maxX, maxX);
            offset.y = Mathf.Clamp(offset.y, -maxY, maxY);

            return center + offset;
        }
        private bool TryUseLockedTarget(out Vector3 _hitPoint, out Vector3 _offset)
        {
            _hitPoint = Vector3.zero;
            _offset = Vector3.up;

            if (!isLocking || lastHit.collider == null)
            {
                return false;
            }

            if (!lastHit.collider.gameObject.activeInHierarchy)
            {
                return false;
            }
            int lastHitLayer = lastHit.collider.gameObject.layer;
            if (((1 << lastHitLayer) & playerModel.LockLayer) == 0)
            {
                return false;
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(lastHit.transform.position);
            if (screenPos.z <= 0 ||
                screenPos.x < 0 || screenPos.x > Screen.width ||
                screenPos.y < 0 || screenPos.y > Screen.height)
            {
                return false;
            }

            _hitPoint = lastHit.collider.bounds.center;
            _offset = lastHit.normal;
            return true;
        }
        private void UpdateAim()
        {
            Transform cameraTransform = Camera.main.transform;
            float crosshairDistance = Vector3.Distance(playerView.transform.position, cameraTransform.position) + 0.1f;

            Vector3 aimDirection = (hitPoint - cameraTransform.position).normalized;
            Vector3 aimPosition = cameraTransform.position + aimDirection * crosshairDistance;
            playerView.GetAimTransform().position = aimPosition;
            playerView.GetAimTransform().forward = aimDirection;


            if (playerWeaponController.GetCurrentWeaponType() != WeaponType.NONE)
            {
                playerWeaponController.GetCurrentWeapon().UpdateWeaponAimPoint(hitPoint);
                playerWeaponController.GetCurrentWeapon().UpdateWeaponAimCrosshairPoint(aimPosition, aimDirection);
            }
        }
        #endregion

        #region Special Actions

        public void ApplyKickback()
        {
            if (playerWeaponController.GetCurrentWeapon().IsAmmoLeft())
            {
                currentSpeed /= playerWeaponController.GetCurrentWeapon().GetModel().WeaponKickBackFactor;
            }
        }
        #endregion

        #region Health & Damage
        public IEnumerator HitImpact(Vector3 _impactForce, int _damage, Collision _hitCollision)
        {
            if (!isRecentlyAttacked)
            {
                DecreaseHealth(_damage);
            }
            isRecentlyAttacked = true;

            var hitPoint = _hitCollision.contacts[0].point;
            if (currentHealth != 0)
            {
                playerMovementStateMachine.ChangeState(PlayerMovementState.HURT);
            }
            else
            {
                playerMovementStateMachine.ChangeState(PlayerMovementState.DEAD);
                yield return new WaitForEndOfFrame();

                Rigidbody impactedRigidbody = _hitCollision.collider.attachedRigidbody;
                if (impactedRigidbody != null)
                {
                    impactedRigidbody.AddForceAtPosition(_impactForce, hitPoint, ForceMode.Impulse);
                }
            }

            yield return new WaitForSeconds(1f);
            isRecentlyAttacked = false;
        }
        private void DecreaseHealth(int _damage)
        {
            currentHealth -= _damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            UpdateHealthUI();
        }
        public void SetHealth(int _healthAmount)
        {
            currentHealth = _healthAmount;
            if (currentHealth > playerModel.MaxHealth)
            {
                currentHealth = playerModel.MaxHealth;
            }
        }
        #endregion

        #region Getters
        public PlayerModel GetModel() => playerModel;
        public PlayerView GetView() => playerView;
        public PlayerAnimationController GetAnimationController() => playerAnimationController;
        public PlayerWeaponController GetWeaponController() => playerWeaponController;
        public PlayerMovementStateMachine GetMovementStateMachine() => playerMovementStateMachine;
        public PlayerActionStateMachine GetActionStateMachine() => playerActionStateMachine;

        public Transform GetTransform() => playerView.transform;
        public Transform GetAimTransform() => playerView.GetAimTransform();
        public Vector3 GetMoveDirection() => moveDirection;
        public float GetCurrentSpeed() => currentSpeed;
        public Vector3 GetXZNormalized(Vector3 _direction)
        {
            _direction.y = 0;
            _direction.Normalize();
            return _direction;
        }
        public LayerMask GetLayerMask()
        {
            int layer = playerView.gameObject.layer;
            return 1 << layer; // Converts layer index to proper LayerMask
        }

        public bool IsGrounded() => Physics.CheckSphere(playerView.transform.position,
            playerModel.GroundCheckDistance, playerModel.GroundLayer);
        public bool IsRunning { get; private set; }
        public bool IsFiring { get; set; }
        public bool IsReloading { get; set; }
        #endregion
    }
}