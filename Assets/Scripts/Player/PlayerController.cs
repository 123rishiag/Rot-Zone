
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

    private float verticalVelocity = 0f;
    private bool isGrounded = false;

    private float currentSpeed = 0f;
    private Vector3 moveDirection;
    private Vector3 lastNonZeroMoveDirection = Vector3.zero;

    void Update()
    {
        ProcessInput();
        ApplyGravity();
        MovePlayer();
        HandleAnimation();
    }

    #region Movement
    void ProcessInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 camForward = cameraController.GetTransform().forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraController.GetTransform().right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 targetDirection = (camForward * vertical + camRight * horizontal).normalized;

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

        RotatePlayer(inputDir, camForward);
        UpdateSpeed(inputDir, isRunning);
    }

    void RotatePlayer(Vector3 _inputDir, Vector3 _cameraForward)
    {
        if (_inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateSpeed(Vector3 _inputDir, bool _isRunning)
    {
        float targetSpeed = _inputDir != Vector3.zero ? (_isRunning ? runSpeed : walkSpeed) : 0f;

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

    void MovePlayer()
    {
        Vector3 move = (moveDirection * currentSpeed);
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }
    #endregion

    #region Animation
    void HandleAnimation()
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
}