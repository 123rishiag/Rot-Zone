using ServiceLocator.Controls;
using ServiceLocator.Utility;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
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
        private PlayerWeaponVisualController playerWeaponVisualController;

        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine playerMovementStateMachine;
        private PlayerActionStateMachine playerActionStateMachine;

        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        private float verticalVelocity;
        private float currentSpeed;

        private Vector3 inputDirection;

        private Vector2 aimPosition;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;

        public PlayerController(PlayerData _playerData, PlayerView _playerPrefab,
            InputService _inputService, CameraService _cameraService, WeaponService _weaponService)
        {
            // Setting Variables
            playerModel = new PlayerModel(_playerData);
            playerView = Object.Instantiate(_playerPrefab).GetComponent<PlayerView>();
            playerView.Init();
            playerAnimationController = new PlayerAnimationController(playerView.GetAnimator(), this);
            playerWeaponVisualController = new PlayerWeaponVisualController(this, _weaponService);

            // Setting Services
            inputService = _inputService;
            cameraService = _cameraService;

            // Setting Variables
            CreateStateMachine();
            SetVariables();
        }

        private void CreateStateMachine()
        {
            Owner = this;
            playerMovementStateMachine = new PlayerMovementStateMachine(this);
            playerActionStateMachine = new PlayerActionStateMachine(this);
        }

        private void SetVariables()
        {
            // Setting Variables
            playerMovementStateMachine.ChangeState(PlayerMovementState.IDLE);
            playerActionStateMachine.ChangeState(PlayerActionState.NONE);

            lastMoveDirection = Vector3.zero;
            verticalVelocity = 0f;
            currentSpeed = 0f;

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

            inputControls.Player.WeaponOne.started += ctx => playerWeaponVisualController.EquipWeapon(WeaponType.PISTOL);
            inputControls.Player.WeaponTwo.started += ctx => playerWeaponVisualController.EquipWeapon(WeaponType.RIFLE);
            inputControls.Player.WeaponThree.started += ctx => playerWeaponVisualController.EquipWeapon(WeaponType.SHOTGUN);
            inputControls.Player.WeaponStow.started += ctx => playerWeaponVisualController.EquipWeapon(WeaponType.NONE);
            inputControls.Player.WeaponReload.started += ctx => playerWeaponVisualController.ReloadWeapon();

            inputControls.Game.Pause.started += ctx => Time.timeScale = 0f;

            inputControls.Player.MousePosition.performed += ctx => aimPosition = ctx.ReadValue<Vector2>();
            inputControls.Player.MousePosition.canceled += ctx => aimPosition = Vector2.zero;
        }
        #endregion

        #region Player Movement & Action
        public void UpdateMovementVariables()
        {
            UpdateDirection();
            ApplyGravity();
            UpdateSpeed();
        }
        public void UpdateActionVariables()
        {
            AimTowardsMouse();
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

                // If player is unarmed, rotate based on camera
                if (playerWeaponVisualController.GetCurrentWeapon() == WeaponType.NONE)
                {
                    RotatePlayerTowards(cameraService.GetCameraForwardXZNormalized());
                }

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
        private void RotatePlayerTowards(Vector3 _direction)
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
        private void AimTowardsMouse()
        {
            if (playerWeaponVisualController.GetCurrentWeapon() != WeaponType.NONE)
            {
                playerView.GetAimTransform().gameObject.SetActive(true);

                Ray ray = Camera.main.ScreenPointToRay(aimPosition);
                Vector3 aimTarget;

                if (Physics.Raycast(ray, out var hitInfo, playerModel.AimMaxDistance, playerModel.AimLayer))
                {
                    aimTarget = hitInfo.point;
                }
                else
                {
                    aimTarget = ray.GetPoint(playerModel.AimMaxDistance);
                }

                playerView.GetAimTransform().position = aimTarget;

                Vector3 direction = (aimTarget - playerView.transform.position).normalized;
                direction.y = 0f;

                if (direction != Vector3.zero)
                    RotatePlayerTowards(direction);
            }
            else
            {
                playerView.GetAimTransform().gameObject.SetActive(false);
                playerView.GetAimTransform().localPosition = playerModel.AimTransformDefaultPosition;
            }
        }
        #endregion

        #region Getters
        public PlayerModel GetModel() => playerModel;
        public PlayerView GetView() => playerView;
        public PlayerAnimationController GetAnimationController() => playerAnimationController;
        public PlayerWeaponVisualController GetWeaponVisualController() => playerWeaponVisualController;
        public PlayerMovementStateMachine GetMovementStateMachine() => playerMovementStateMachine;
        public PlayerActionStateMachine GetActionStateMachine() => playerActionStateMachine;
        public Transform GetTransform() => playerView.transform;
        public Vector3 GetMoveDirection() => moveDirection;
        public float GetCurrentSpeed() => currentSpeed;
        public bool IsGrounded() => Physics.CheckSphere(playerView.transform.position,
            playerModel.GroundCheckDistance, playerModel.GroundLayer);
        public bool IsRunning { get; private set; }
        public bool IsFiring { get; private set; }
        #endregion
    }
}