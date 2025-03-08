using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform cameraPivot;
    public float mouseSensitivity = 3f;
    public float minPitch = 30f;
    public float maxPitch = 60f;
    public float defaultDistance = 2.5f;
    public float cameraCollisionRadius = 0.2f;
    public float initialYaw = 0f;
    public float initialPitch = 20f;
    public LayerMask collisionLayers;

    // Private Variables
    private float yaw;
    private float pitch;

    private void Start()
    {
        // Setting Variables
        yaw = initialYaw;
        pitch = initialPitch;
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
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
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
