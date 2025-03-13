using ServiceLocator.Controls;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ServiceLocator.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private WeaponIKData[] weaponIKDatas;
        [SerializeField] private MultiAimConstraint rightHandAimConstraint;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;

        [Header("Aim Settings")]
        [SerializeField] private Transform aimTransform;

        [Header("Camera Settings")]
        [SerializeField] private Transform cameraPivot;

        // Private Variables
        private PlayerConfig playerConfig;
        private CharacterController characterController;
        private Animator animator;

        private PlayerMovementState playerMovementState;
        private PlayerMovementState playerLastMovementState;

        private PlayerActionState playerActionState;
        private PlayerActionState playerLastActionState;

        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        private float verticalVelocity;
        private float currentSpeed;
        private float yawRotation;
        private bool isGrounded;

        private WeaponType currentWeaponType;
        private int weaponAnimationLayerIndex;
        private float weaponAnimationLayerWeight;
        private Dictionary<WeaponType, GameObject> weaponPrefabs;
        private Transform currentWeaponHolder;

        private Vector2 aimPosition;

        private Vector3 inputDirection;
        private bool isRunning;
        private bool isFiring;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;
        private WeaponService weaponService;

        public void Init(PlayerConfig _playerConfig,
            InputService _inputService, CameraService _cameraService, WeaponService _weaponService)
        {
            // Setting Services
            inputService = _inputService;
            cameraService = _cameraService;
            weaponService = _weaponService;

            // Setting Elements
            playerConfig = _playerConfig;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            weaponPrefabs = new Dictionary<WeaponType, GameObject>();

            SetVariables();
        }

        private void SetVariables()
        {
            // Setting Variables
            playerMovementState = PlayerMovementState.NONE;
            playerActionState = PlayerActionState.NONE;
            ChangeMovementState(PlayerMovementState.IDLE);
            ChangeActionState(PlayerActionState.NONE);

            lastMoveDirection = Vector3.zero;
            verticalVelocity = 0f;
            currentSpeed = 0f;
            yawRotation = 0f;
            isGrounded = false;

            currentWeaponType = WeaponType.NONE;
            weaponAnimationLayerIndex = 0;
            weaponAnimationLayerWeight = 0f;
            SetCurrentWeaponSetting();
            CreateWeapons();

            AssignInputs();
        }

        #region Input
        private void AssignInputs()
        {
            // Not taking inputs if Player is Falling
            if (playerMovementState == PlayerMovementState.FALL)
                return;

            // Camera Inputs
            InputControls inputControls = inputService.GetInputControls();

            inputControls.Player.Movement.performed += ctx =>
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                inputDirection = new Vector3(input.x, 0f, input.y);
            };
            inputControls.Player.Movement.canceled += ctx => inputDirection = Vector3.zero;

            inputControls.Player.Run.performed += ctx => isRunning = true;
            inputControls.Player.Run.canceled += ctx => isRunning = false;

            inputControls.Player.Run.performed += ctx => isRunning = true;
            inputControls.Player.Run.canceled += ctx => isRunning = false;

            inputControls.Player.Fire.performed += ctx => isFiring = true;
            inputControls.Player.Fire.canceled += ctx => isFiring = false;

            inputControls.Player.WeaponOne.started += ctx => EquipWeapon(WeaponType.PISTOL);
            inputControls.Player.WeaponTwo.started += ctx => EquipWeapon(WeaponType.RIFLE);
            inputControls.Player.WeaponThree.started += ctx => EquipWeapon(WeaponType.SHOTGUN);
            inputControls.Player.WeaponStow.started += ctx => EquipWeapon(WeaponType.NONE);

            inputControls.Game.Pause.started += ctx => Time.timeScale = 0f;

            inputControls.Player.MousePosition.performed += ctx => aimPosition = ctx.ReadValue<Vector2>();
            inputControls.Player.MousePosition.canceled += ctx => aimPosition = Vector2.zero;
        }
        #endregion

        public void UpdateData()
        {
            UpdateMovementState();
            UpdateActionState();
            MovePlayer();

            UpdateAnimationLayerWeight();
            UpdateAnimationParameters();
            UpdateMovementAnimation();
            UpdateActionAnimation();

            AimTowardsMouse();
        }

        #region Player State Handling
        private void UpdateMovementState()
        {
            if (!isGrounded)
            {
                ChangeMovementState(PlayerMovementState.FALL);
            }
            else if (currentSpeed == 0)
            {
                ChangeMovementState(PlayerMovementState.IDLE);
            }
            else if (currentSpeed <= playerConfig.walkSpeed)
            {
                ChangeMovementState(PlayerMovementState.WALK);
            }
            else
            {
                ChangeMovementState(PlayerMovementState.RUN);
            }
        }
        private void UpdateActionState()
        {
            if (currentWeaponType != WeaponType.NONE && isFiring)
            {
                ChangeActionState(PlayerActionState.FIRE);
            }
            else if (currentWeaponType != WeaponType.NONE)
            {
                ChangeActionState(PlayerActionState.AIM);
            }
            else
            {
                ChangeActionState(PlayerActionState.NONE);
            }
        }
        private void ChangeMovementState(PlayerMovementState _playerMovementState)
        {
            playerLastMovementState = playerMovementState;
            playerMovementState = _playerMovementState;
        }
        private void ChangeActionState(PlayerActionState _playerActionState)
        {
            playerLastActionState = playerActionState;
            playerActionState = _playerActionState;
        }
        #endregion

        #region Player Controls
        private void MovePlayer()
        {
            UpdateDirection();
            ApplyGravity();
            UpdateSpeed();
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
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
                if (currentWeaponType == WeaponType.NONE)
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
                Time.deltaTime * playerConfig.directionSmoothSpeed);
        }
        private void RotatePlayerTowards(Vector3 _direction)
        {
            // Rotate Player Towards Camera, when player is not falling
            if (playerMovementState == PlayerMovementState.FALL)
                return;

            if (_direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime *
                playerConfig.rotationSpeed);
        }
        private void ApplyGravity()
        {
            isGrounded = Physics.CheckSphere(transform.position, playerConfig.groundCheckDistance,
                playerConfig.groundLayer);

            // If Player is on Ground, give some velocity to keep the player grounded,
            // else reduce velocity by gravity Scale Factor
            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity -= playerConfig.gravityFactor * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;
        }
        private void UpdateSpeed()
        {
            // Fetching target speed based on inputs
            float targetSpeed = inputDirection != Vector3.zero ?
                (isRunning ? playerConfig.runSpeed : playerConfig.walkSpeed) : 0f;

            // Applying Acceleration and DeAcceleration
            if (currentSpeed < targetSpeed)
            {
                currentSpeed += playerConfig.accelerationFactor * Time.deltaTime;

                // Clamping Current Speed to Target Speed, should not go above target speed while accelerating
                if (currentSpeed > targetSpeed)
                    currentSpeed = targetSpeed;
            }
            else if (currentSpeed > targetSpeed)
            {
                currentSpeed -= playerConfig.decelerationFactor * Time.deltaTime;

                // Clamping Current Speed to Target Speed, should not go below target speed while deaccelerating
                if (currentSpeed < targetSpeed)
                    currentSpeed = targetSpeed;
            }
        }
        private void AimTowardsMouse()
        {
            if (playerActionState == PlayerActionState.AIM || playerActionState == PlayerActionState.FIRE)
            {
                aimTransform.gameObject.SetActive(true);

                Ray ray = Camera.main.ScreenPointToRay(aimPosition);
                Vector3 aimTarget;

                if (Physics.Raycast(ray, out var hitInfo, playerConfig.aimMaxDistance, playerConfig.aimLayer))
                {
                    aimTarget = hitInfo.point;
                }
                else
                {
                    aimTarget = ray.GetPoint(playerConfig.aimMaxDistance);
                }

                aimTransform.position = aimTarget;

                Vector3 direction = (aimTarget - transform.position).normalized;
                direction.y = 0f;

                if (direction != Vector3.zero)
                    RotatePlayerTowards(direction);
            }
            else
            {
                aimTransform.gameObject.SetActive(false);
                aimTransform.localPosition = playerConfig.aimTransformDefaultPosition;
            }
        }
        #endregion

        #region Animation
        private void UpdateMovementAnimation()
        {
            if (playerLastMovementState == playerMovementState)
                return;

            switch (playerMovementState)
            {
                case PlayerMovementState.IDLE:
                case PlayerMovementState.WALK:
                case PlayerMovementState.RUN:
                    animator.Play("Movement Locomotion");
                    break;
                case PlayerMovementState.FALL:
                    animator.Play("Fall");
                    break;
                default:
                    animator.Play("TPose");
                    break;
            }
        }
        private void UpdateActionAnimation()
        {
            if (playerLastActionState == playerActionState)
                return;

            weaponAnimationLayerWeight = 1f;

            switch (playerActionState)
            {
                case PlayerActionState.FIRE:
                    animator.Play("Weapon_Fire");
                    break;
                case PlayerActionState.AIM:
                    animator.Play("Weapon_Idle");
                    break;
                default:
                    weaponAnimationLayerWeight = 0f;
                    break;
            }
        }
        private void UpdateAnimationParameters()
        {
            // We only want the player's horizontal rotation to affect animation, yaw means horizontal
            yawRotation = transform.eulerAngles.y;
            Quaternion yawOnlyRotation = Quaternion.Euler(0, yawRotation, 0);

            // To convert world coordinates into the local coordinates of an object,
            // we multiply the world coordinates with the inverse of the object's localRotation.
            // To convert local coordinates of an object into world coordinates,
            // we multiply the local coordinates with the object's localRotation.
            Vector3 cameraRelativeMoveDir = Quaternion.Inverse(yawOnlyRotation) * moveDirection;

            // Updating the "moveX" and "moveY" parameter to smoothly blend between
            // forward, backward, left, right and diagonal movement in the Animator.
            animator.SetFloat("moveX", cameraRelativeMoveDir.x, 0.1f, Time.deltaTime);
            animator.SetFloat("moveZ", cameraRelativeMoveDir.z, 0.1f, Time.deltaTime);

            float normalizedSpeed = 0f;

            //Mathf.InverseLerp(minRange, maxRange, value) means what percentage does values lies between 0 and 1.
            // Ex - minRange = 10, maxRange = 20, value = 15, result = .5;
            if (currentSpeed > 0f && currentSpeed <= playerConfig.walkSpeed)
            {
                // Ex - minRange = 0, maxRange = walkSpeed = 5, currentSpeed = 2.5, result = 0.5
                // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0) 
                normalizedSpeed = Mathf.InverseLerp(0f, playerConfig.walkSpeed, currentSpeed) * 0.5f + 0f;
            }
            else if (currentSpeed > playerConfig.walkSpeed)
            {
                // Ex - minRange = 5, maxRange = walkSpeed = 10, currentSpeed = 7.5, result = 0.5
                // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0.5) 
                normalizedSpeed = Mathf.InverseLerp(playerConfig.walkSpeed, playerConfig.runSpeed, currentSpeed)
                    * 0.5f + 0.5f;
            }

            // Updating the "speed" parameter to smoothly blend between idle, walk, and run in the Animator.
            animator.SetFloat("speed", normalizedSpeed, 0.1f, Time.deltaTime);
        }
        #endregion

        #region Weapon
        private void CreateWeapons()
        {
            foreach (WeaponIKData weaponIKData in weaponIKDatas)
            {
                GameObject weaponPrefab = weaponService.CreateWeapon(weaponIKData.weaponType);
                weaponPrefabs[weaponIKData.weaponType] = weaponPrefab;

                Transform parentTransform = GetWeaponIKData(weaponIKData.weaponType).weaponTypeHolder;
                weaponPrefab.transform.SetParent(parentTransform);

                AttachWeaponToRightHand(weaponIKData.weaponType);
            }
            SwitchOffWeapons();
        }
        private void AttachWeaponToRightHand(WeaponType _weaponType)
        {
            WeaponIKData weaponIKData = GetWeaponIKData(_weaponType);
            Transform rightHand_TargetTransform = weaponIKData.weaponTypeHolder.transform.Find("RightHand_Target");

            weaponPrefabs[_weaponType].transform.position = rightHand_TargetTransform.position;
            weaponPrefabs[_weaponType].transform.rotation = rightHand_TargetTransform.rotation;
            weaponPrefabs[_weaponType].transform.localScale = rightHand_TargetTransform.localScale;
        }
        private void EquipWeapon(WeaponType _weaponType)
        {
            SwitchOffWeapons();
            currentWeaponType = _weaponType;

            if (currentWeaponType != WeaponType.NONE)
            {
                weaponPrefabs[_weaponType].gameObject.SetActive(true);

                WeaponIKData weaponIKData = GetWeaponIKData(_weaponType);
                currentWeaponHolder = weaponIKData.weaponTypeHolder;

                AttachLeftHandToWeapon(_weaponType);
            }

            SetCurrentWeaponSetting();
        }
        private void SwitchOffWeapons()
        {
            foreach (WeaponIKData weaponIKData in weaponIKDatas)
            {
                weaponPrefabs[weaponIKData.weaponType].gameObject.SetActive(false);
            }
        }

        private void AttachLeftHandToWeapon(WeaponType _weaponType)
        {
            Transform currentLeftHand_Target = currentWeaponHolder.transform.Find("LeftHand_Target");
            Transform currentLeftHand_Hint = currentWeaponHolder.transform.Find("LeftHand_Hint");

            leftHandIK.data.target.localPosition = currentLeftHand_Target.localPosition;
            leftHandIK.data.target.localRotation = currentLeftHand_Target.localRotation;
            leftHandIK.data.target.localScale = currentLeftHand_Target.localScale;

            leftHandIK.data.hint.localPosition = currentLeftHand_Hint.localPosition;
            leftHandIK.data.hint.localRotation = currentLeftHand_Hint.localRotation;
            leftHandIK.data.hint.localScale = currentLeftHand_Hint.localScale;
        }
        private void SetCurrentWeaponSetting()
        {
            switch (currentWeaponType)
            {
                case WeaponType.PISTOL:
                    SetAnimationLayer(1);
                    SetIKWeight(1f);
                    break;
                case WeaponType.RIFLE:
                    SetAnimationLayer(2);
                    SetIKWeight(1f);
                    break;
                case WeaponType.SHOTGUN:
                    SetAnimationLayer(3);
                    SetIKWeight(1f);
                    break;
                case WeaponType.NONE:
                default:
                    SetAnimationLayer(0);
                    SetIKWeight(0f);
                    break;
            }
        }
        private void SetIKWeight(float _weight)
        {
            leftHandIK.weight = _weight;
            rightHandAimConstraint.weight = _weight;
        }
        private void UpdateAnimationLayerWeight()
        {
            if (weaponAnimationLayerIndex != -1) // Ensuring that the layer exists
            {
                float currentWeight = animator.GetLayerWeight(weaponAnimationLayerIndex);
                float targetWeight = Mathf.Lerp(currentWeight, weaponAnimationLayerWeight,
                    Time.deltaTime * playerConfig.weaponLayerWeightChangeFactor);
                animator.SetLayerWeight(weaponAnimationLayerIndex, targetWeight);
            }
        }
        private void SetAnimationLayer(int _layerIndex)
        {
            for (int i = 1; i < animator.layerCount; ++i)
            {
                animator.SetLayerWeight(i, 0f);
            }

            weaponAnimationLayerIndex = _layerIndex;
            weaponAnimationLayerWeight = 1f;
        }
        private WeaponIKData GetWeaponIKData(WeaponType _weaponType) =>
            Array.Find(weaponIKDatas, w => w.weaponType == _weaponType);

        #endregion

        // Getters
        public Transform GetCameraPivot() => cameraPivot;
    }
}