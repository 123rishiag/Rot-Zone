using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;

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

    // Private Variables
    private CharacterController characterController;
    private Animator animator;

    private PlayerState playerState;
    private PlayerState playerLastState;

    private Vector3 inputDir;
    private bool isRunning;

    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private float verticalVelocity;
    private float currentSpeed;
    private float yawRotation;
    private bool isGrounded;

    private void Awake()
    {
        // Setting Variables
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // Setting Variables
        playerState = PlayerState.NONE;
        ChangeState(PlayerState.IDLE);
        lastMoveDirection = Vector3.zero;
        verticalVelocity = 0f;
        currentSpeed = 0f;
        yawRotation = 0f;
        isGrounded = false;
    }

    private void Update()
    {
        GetInput();
        UpdatePlayerState();
        MovePlayer();
        UpdateAnimation();
    }

    #region Input
    private void GetInput()
    {
        // Disable input while falling
        if (playerState == PlayerState.FALL)
            return;

        // Movement Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        // Run Input
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }
    #endregion

    #region Player State
    private void UpdatePlayerState()
    {
        if (!isGrounded)
        {
            ChangeState(PlayerState.FALL);
        }
        else if (currentSpeed == 0)
        {
            ChangeState(PlayerState.IDLE);
        }
        else if (currentSpeed <= walkSpeed)
        {
            ChangeState(PlayerState.WALK);
        }
        else
        {
            ChangeState(PlayerState.RUN);
        }
    }
    private void ChangeState(PlayerState _playerState)
    {
        playerLastState = playerState;
        playerState = _playerState;
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
        if (inputDir.magnitude > 0.1f)
        {
            // Fetching Target Direction where player is trying to move in world based on input and camera
            targetDirection = (cameraController.GetCameraForwardXZNormalized() * inputDir.z +
                cameraController.GetCameraRightXZNormalized() * inputDir.x).normalized;

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
        if (playerState == PlayerState.FALL)
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
        // Fetching target spped based on inputs
        float targetSpeed = inputDir != Vector3.zero ? (isRunning ? runSpeed : walkSpeed) : 0f;

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
    private void UpdateAnimation()
    {
        UpdateAnimationParameters();

        if (playerLastState == playerState)
            return;

        switch (playerState)
        {
            case PlayerState.IDLE:
            case PlayerState.WALK:
            case PlayerState.RUN:
                animator.Play("Movement Locomotion");
                break;
            case PlayerState.FALL:
                animator.Play("Fall");
                break;
            default:
                animator.Play("TPose");
                break;
        }
    }
    private void UpdateAnimationParameters()
    {
        // We only want the camera's horizontal rotation to affect animation, yaw means horizontal
        // but we want camera rotation of last point where movement input was given as we dont want
        // camera local rotation to keep changing animation parameters while deacceleration
        // which will lead to player's movement animation out of sync with movement direction
        if (inputDir.magnitude > 0.1f)
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
}

public enum PlayerState
{
    NONE,
    IDLE,
    WALK,
    RUN,
    FALL,
}