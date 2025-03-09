using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private InputController inputController;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float minPitch = 30f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float defaultDistance = 2.5f;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private float initialYaw = 0f;
    [SerializeField] private float initialPitch = 20f;
    [SerializeField] private LayerMask collisionLayers;

    // Private Variables
    private float yaw;
    private float pitch;

    private Vector2 cameraMouseDelta;

    private void Start()
    {
        // Setting Variables
        yaw = initialYaw;
        pitch = initialPitch;

        AssignInputs();
    }

    private void AssignInputs()
    {
        // Camera Inputs
        InputControls inputControls = inputController.GetInputControls();

        inputControls.Camera.MouseDelta.performed += ctx => cameraMouseDelta = ctx.ReadValue<Vector2>();
        inputControls.Camera.MouseDelta.canceled += ctx => cameraMouseDelta = Vector2.zero;
    }

    private void Update()
    {
        HandleCameraRotation();
    }

    private void LateUpdate()
    {
        HandleCameraPosition();
    }

    private void HandleCameraRotation()
    {
        // Setting horizontal and vertical rotation based on mouse movement after clamping it on vertical axis
        yaw += cameraMouseDelta.x * mouseSensitivity;
        pitch -= cameraMouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -minPitch, maxPitch);

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleCameraPosition()
    {
        // Setting Camera Position based on camera pivot on player
        Vector3 desiredPos = cameraPivot.position - cameraPivot.forward * defaultDistance;

        // If Camera collides with a world object like ground, it will stop there, other wise the desired position
        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, -cameraPivot.forward, out RaycastHit hit, defaultDistance, collisionLayers))
        {
            transform.position = hit.point + cameraPivot.forward * cameraCollisionRadius;
        }
        else
        {
            transform.position = desiredPos;
        }

        // So that camera look towards the player, not just rotate orbitally
        transform.LookAt(cameraPivot.position);
    }

    // Getters
    public Transform GetTransform() => transform;
    public Vector3 GetCameraForwardXZNormalized() => GetCameraXZNormalizedVector(transform.forward);
    public Vector3 GetCameraRightXZNormalized() => GetCameraXZNormalizedVector(transform.right);
    private Vector3 GetCameraXZNormalizedVector(Vector3 _cameraVector)
    {
        // Disabling Camera Height as for movement of player, camera's height doesn't matter
        _cameraVector.y = 0;

        // Normalizing Camera Vectors as magnitude is not needed only direction
        _cameraVector.Normalize();

        return _cameraVector;
    }
}
