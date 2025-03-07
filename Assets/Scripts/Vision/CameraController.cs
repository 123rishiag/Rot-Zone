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
}
