using ServiceLocator.Controls;
using ServiceLocator.Utility;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ServiceLocator.Player
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

        private Vector2 aimPosition;

        private int currentHealth;

        private bool isRecentlyAttacked;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;

        public PlayerController(PlayerData _playerData, PlayerView _playerPrefab, Vector3 _spawnPosition,
            InputService _inputService, CameraService _cameraService, WeaponService _weaponService)
        {
            // Setting Variables
            playerModel = new PlayerModel(_playerData);
            playerView = Object.Instantiate(_playerPrefab).GetComponent<PlayerView>();
            playerView.Init(this);
            playerAnimationController = new PlayerAnimationController(playerView.GetAnimator(), this);
            playerWeaponController = new PlayerWeaponController(this, _weaponService);

            // Setting Services
            inputService = _inputService;
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
            currentHealth = playerModel.MaxHealth;
            isRecentlyAttacked = false;

            playerView.SetPosition(_spawnPosition);
            playerView.SetRagDollActive(true);
            playerView.ShowView();

            AssignInputs();
        }

        public void Update()
        {
            playerMovementStateMachine.Update();
            playerActionStateMachine.Update();

            playerAnimationController.UpdateAnimation();
        }

        #region Input
        private void AssignInputs()
        {
            // Not taking inputs if Player is Falling
            if (playerMovementStateMachine.GetCurrentState() == PlayerMovementState.FALL)
                return;

            // Camera Inputs
            InputControls inputControls = inputService.GetInputControls();

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

            inputControls.Player.WeaponOne.started += ctx => playerWeaponController.EquipWeapon(WeaponType.PISTOL);
            inputControls.Player.WeaponTwo.started += ctx => playerWeaponController.EquipWeapon(WeaponType.RIFLE);
            inputControls.Player.WeaponThree.started += ctx => playerWeaponController.EquipWeapon(WeaponType.SHOTGUN);
            inputControls.Player.WeaponStow.started += ctx => playerWeaponController.EquipWeapon(WeaponType.NONE);
            inputControls.Player.WeaponReload.started += ctx => IsReloading = true;

            inputControls.Game.Pause.started += ctx => Time.timeScale = 0f;

            inputControls.Player.MousePosition.performed += ctx => aimPosition = ctx.ReadValue<Vector2>();
            inputControls.Player.MousePosition.canceled += ctx => aimPosition = Vector2.zero;
        }
        #endregion

        #region Player Movement
        public void UpdateMovementVariables()
        {
            UpdateDirection();
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
                targetDirection = (cameraService.GetCameraForwardXZNormalized() * inputDirection.z +
                    cameraService.GetCameraRightXZNormalized() * inputDirection.x).normalized;

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
        private void RotateTowards(Vector3 _direction)
        {
            // Rotate Player Towards Camera, when player is not falling
            if (playerMovementStateMachine.GetCurrentState() == PlayerMovementState.FALL)
                return;

            if (_direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);

            playerView.transform.rotation = Quaternion.Slerp(playerView.transform.rotation, targetRotation,
                Time.deltaTime * playerModel.RotationSpeed);
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
            if (playerWeaponController.GetCurrentWeaponType() != WeaponType.NONE)
            {
                // Setting Aim Based on Mouse Position
                Ray ray = Camera.main.ScreenPointToRay(aimPosition);

                Vector3 aimTarget =
                    ray.GetPoint(playerWeaponController.GetCurrentWeapon().GetView().GetAimDistance());

                // Setting Offsets for Weapons
                aimTarget = new Vector3(
                    aimTarget.x,
                    aimTarget.y + playerWeaponController.GetCurrentWeaponTransform().weaponVerticalOffeset,
                    aimTarget.z);

                playerView.GetAimTransform().position = aimTarget;

                Vector3 direction = (aimTarget - playerView.transform.position).normalized;
                direction.y = 0f;
                RotateTowards(direction);
            }
            else
            {
                playerView.GetAimTransform().localPosition = playerModel.AimTransformDefaultPosition;
                RotateTowards(cameraService.GetCameraForwardXZNormalized());
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
                currentHealth = 0;
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
        public Vector3 GetMoveDirection() => moveDirection;
        public float GetCurrentSpeed() => currentSpeed;
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