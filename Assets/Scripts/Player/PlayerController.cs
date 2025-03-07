
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controllers")]
    public CameraController cameraController;

    [Header("References")]
    public CharacterController characterController;
    public Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5f;
    public float acceleration = 5f;
    public float deceleration = 2.5f;
    public float directionSmoothSpeed = 10f;
    public float rotationSpeed = 3f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundedCheckDistance = 0.1f;
    public LayerMask groundMask;

    private PlayerState playerState = PlayerState.TPOSE;
    private PlayerState playerLastState;
    private float verticalVelocity = 0f;
    private bool isGrounded = false;

    private float currentSpeed = 0f;
    private Vector3 moveDirection;
    private Vector3 lastNonZeroMoveDirection = Vector3.zero;
    private Vector3 inputDir;

    void Start()
    {
        ChangeState(PlayerState.IDLE);
    }

    void Update()
    {
        ApplyGravity();
        ProcessInput();
        UpdateDirection();
        UpdateSpeed();
        UpdatePlayerState();
        MovePlayer();
        HandleAnimation();
        UpdateAnimationParameters();
    }

    #region Movement

    void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundedCheckDistance, groundMask);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }
    void ProcessInput()
    {
        if (playerState == PlayerState.FALL)
            return; // Disable input while falling

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(horizontal, 0f, vertical).normalized;
    }

    void UpdateDirection()
    {
        Vector3 cameraForward = cameraController.GetTransform().forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraController.GetTransform().right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 targetDirection = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;

        if (inputDir.magnitude > 0.01f)
        {
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * directionSmoothSpeed);
            lastNonZeroMoveDirection = moveDirection;
        }
        else if (currentSpeed > 0.1f)
        {
            moveDirection = Vector3.Lerp(moveDirection, lastNonZeroMoveDirection, Time.deltaTime * directionSmoothSpeed);
        }
        else
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime * directionSmoothSpeed);
        }

        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateSpeed()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = inputDir != Vector3.zero ? (isRunning ? runSpeed : walkSpeed) : 0f;

        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            if (currentSpeed > targetSpeed)
                currentSpeed = targetSpeed;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= deceleration * Time.deltaTime;
            if (currentSpeed < targetSpeed)
                currentSpeed = targetSpeed;
        }
    }

    void MovePlayer()
    {
        Vector3 move = (moveDirection * currentSpeed);
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }
    #endregion

    #region Player State
    void UpdatePlayerState()
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
    #endregion

    #region Animation
    void HandleAnimation()
    {
        if (playerLastState == playerState)
            return;

        switch(playerState)
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
    void UpdateAnimationParameters()
    {
        Quaternion yawOnlyRotation = Quaternion.Euler(0, cameraController.GetTransform().eulerAngles.y, 0);
        Vector3 cameraRelativeMoveDir = Quaternion.Inverse(yawOnlyRotation) * moveDirection;

        animator.SetFloat("moveX", cameraRelativeMoveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat("moveZ", cameraRelativeMoveDir.z, 0.1f, Time.deltaTime);

        float normalizedSpeed = 0f;

        if (currentSpeed > 0f && currentSpeed <= walkSpeed)
        {
            normalizedSpeed = Mathf.InverseLerp(0f, walkSpeed, currentSpeed) * 0.5f;
        }
        else if (currentSpeed > walkSpeed)
        {
            normalizedSpeed = Mathf.InverseLerp(walkSpeed, runSpeed, currentSpeed) * 0.5f + 0.5f;
        }

        animator.SetFloat("speed", normalizedSpeed, 0.1f, Time.deltaTime);
    }
    #endregion

    void ChangeState(PlayerState _playerState)
    {
        playerLastState = playerState;
        playerState = _playerState;
    }
}

public enum PlayerState
{
    TPOSE,
    IDLE,
    WALK,
    RUN,
    FALL,
}