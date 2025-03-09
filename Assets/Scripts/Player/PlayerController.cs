using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private InputController inputController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private WeaponController weaponController;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float accelerationFactor = 5f;
    [SerializeField] private float decelerationFactor = 2.5f;
    [SerializeField] private float directionSmoothSpeed = 10f;
    [SerializeField] private float rotationSpeed = 3f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityFactor = 9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Weapon Settings")]
    [SerializeField] private float weaponLayerWeightChangeFactor = 10f;
    [SerializeField] private WeaponTransform[] weaponTransforms;

    // Private Variables
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

    private int weaponAnimationLayerIndex;
    private float weaponAnimationLayerWeight;
    private List<WeaponType> weaponTypes;
    private List<GameObject> weapons;

    private Vector3 inputDirection;
    private bool isRunning;
    private bool isAiming;
    private bool isFiring;

    private void Awake()
    {
        // Setting Variables
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        weaponTypes = new List<WeaponType>();
        weapons = new List<GameObject>();
    }

    private void Start()
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

        weaponAnimationLayerIndex = 1;
        weaponAnimationLayerWeight = 0f;
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
        InputControls inputControls = inputController.GetInputControls();

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

        inputControls.Player.Aim.performed += ctx => isAiming = true;
        inputControls.Player.Aim.canceled += ctx => isAiming = false;

        inputControls.Player.Fire.performed += ctx => isFiring = true;
        inputControls.Player.Fire.canceled += ctx => isFiring = false;

        inputControls.Player.WeaponOne.started += ctx => EquipWeapon(WeaponType.PISTOL);
        inputControls.Player.WeaponTwo.started += ctx => EquipWeapon(WeaponType.RIFLE);
        inputControls.Player.WeaponThree.started += ctx => EquipWeapon(WeaponType.SHOTGUN);
        inputControls.Player.WeaponStow.started += ctx => EquipWeapon(WeaponType.NONE);

        inputControls.Game.Pause.started += ctx => Time.timeScale = 0f;
    }
    #endregion

    private void Update()
    {
        UpdateMovementState();
        UpdateActionState();
        MovePlayer();

        UpdateAnimationLayerWeight();
        UpdateAnimationParameters();
        UpdateMovementAnimation();
        UpdateActionAnimation();
    }

    #region Player Movement State
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
        else if (currentSpeed <= walkSpeed)
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
        if (isFiring)
        {
            ChangeActionState(PlayerActionState.FIRE);
        }
        else if (isAiming)
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

    #region Movement
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
            targetDirection = (cameraController.GetCameraForwardXZNormalized() * inputDirection.z +
                cameraController.GetCameraRightXZNormalized() * inputDirection.x).normalized;

            RotatePlayerTowardsCamera();
            lastMoveDirection = moveDirection;
        }
        else if (currentSpeed > 0.1f)
        {
            targetDirection = lastMoveDirection;
        }

        //  Smoothly changing move Direction towards target Direction
        moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * directionSmoothSpeed);
    }
    private void RotatePlayerTowardsCamera()
    {
        // Rotate Player Towards Camera, when player is not falling
        if (playerMovementState == PlayerMovementState.FALL)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraController.GetCameraForwardXZNormalized());
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);

        // If Player is on Ground, give some velocity to keep the player grounded,
        // else reduce velocity by gravity Scale Factor
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravityFactor * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
    }
    private void UpdateSpeed()
    {
        // Fetching target speed based on inputs
        float targetSpeed = inputDirection != Vector3.zero ? (isRunning ? runSpeed : walkSpeed) : 0f;

        // Applying Acceleration and DeAcceleration
        if (currentSpeed < targetSpeed)
        {
            currentSpeed += accelerationFactor * Time.deltaTime;

            // Clamping Current Speed to Target Speed, should not go above target speed while accelerating
            if (currentSpeed > targetSpeed)
                currentSpeed = targetSpeed;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= decelerationFactor * Time.deltaTime;

            // Clamping Current Speed to Target Speed, should not go below target speed while deaccelerating
            if (currentSpeed < targetSpeed)
                currentSpeed = targetSpeed;
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
                animator.Play("Rifle_Fire");
                break;
            case PlayerActionState.AIM:
                animator.Play("Rifle_Idle");
                break;
            default:
                weaponAnimationLayerWeight = 0f;
                break;
        }
    }
    private void UpdateAnimationLayerWeight()
    {
        if (weaponAnimationLayerIndex != -1) // Ensuring that the layer exists
        {
            float currentWeight = animator.GetLayerWeight(weaponAnimationLayerIndex);
            float targetWeight = Mathf.Lerp(currentWeight, weaponAnimationLayerWeight,
                Time.deltaTime * weaponLayerWeightChangeFactor);
            animator.SetLayerWeight(weaponAnimationLayerIndex, targetWeight);
        }
    }
    private void UpdateAnimationParameters()
    {
        // We only want the camera's horizontal rotation to affect animation, yaw means horizontal
        // but we want camera rotation of last point where movement input was given as we dont want
        // camera local rotation to keep changing animation parameters while deacceleration
        // which will lead to player's movement animation out of sync with movement direction
        if (inputDirection.magnitude > 0.1f)
        {
            yawRotation = cameraController.GetTransform().eulerAngles.y;
        }
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
        if (currentSpeed > 0f && currentSpeed <= walkSpeed)
        {
            // Ex - minRange = 0, maxRange = walkSpeed = 5, currentSpeed = 2.5, result = 0.5
            // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0) 
            normalizedSpeed = Mathf.InverseLerp(0f, walkSpeed, currentSpeed) * 0.5f + 0f;
        }
        else if (currentSpeed > walkSpeed)
        {
            // Ex - minRange = 5, maxRange = walkSpeed = 10, currentSpeed = 7.5, result = 0.5
            // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0.5) 
            normalizedSpeed = Mathf.InverseLerp(walkSpeed, runSpeed, currentSpeed) * 0.5f + 0.5f;
        }

        // Updating the "speed" parameter to smoothly blend between idle, walk, and run in the Animator.
        animator.SetFloat("speed", normalizedSpeed, 0.1f, Time.deltaTime);
    }
    #endregion

    #region Weapon
    private void CreateWeapons()
    {
        foreach (WeaponTransform weaponTransform in weaponTransforms)
        {
            GameObject weapon =
                weaponController.CreateWeapon(weaponTransform.weaponType, weaponTransform.weaponParentTransform);

            weaponTypes.Add(weaponTransform.weaponType);
            weapons.Add(weapon);
        }
    }

    private void EquipWeapon(WeaponType _weaponType)
    {
        SwitchOffWeapons();

        if (_weaponType == WeaponType.NONE) return;

        foreach (WeaponTransform weaponTransform in weaponTransforms)
        {
            if (weaponTransform.weaponType == _weaponType)
                weaponTransform.weaponParentTransform.gameObject.SetActive(true);
        }

    }
    private void SwitchOffWeapons()
    {
        foreach (WeaponTransform weaponTransform in weaponTransforms)
        {
            weaponTransform.weaponParentTransform.gameObject.SetActive(false);
        }
    }
    #endregion
}

public enum PlayerMovementState
{
    NONE,
    IDLE,
    WALK,
    RUN,
    FALL,
}

public enum PlayerActionState
{
    NONE,
    AIM,
    FIRE,
}