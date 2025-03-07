using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform cameraPivot;
    public float mouseSensitivity = 3f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public float defaultDistance = 2.5f;
    public float cameraCollisionRadius = 0.2f;
    public LayerMask collisionLayers;

    private float yaw = 0f;
    private float pitch = 20f;

    void Update()
    {
        HandleCameraRotation();
    }

    void LateUpdate()
    {
        HandleCameraPosition();
    }

    void HandleCameraRotation()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleCameraPosition()
    {
        Vector3 desiredPos = cameraPivot.position - cameraPivot.forward * defaultDistance;

        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, -cameraPivot.forward, out RaycastHit hit, defaultDistance, collisionLayers))
        {
            transform.position = hit.point + cameraPivot.forward * cameraCollisionRadius;
        }
        else
        {
            transform.position = desiredPos;
        }

        transform.LookAt(cameraPivot.position);
    }

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
