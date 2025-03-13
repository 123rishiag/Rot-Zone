using ServiceLocator.Controls;
using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Vision
{
    public class CameraService
    {
        // Private Variables
        private CameraConfig cameraConfig;
        private Camera mainCamera;

        private float yaw;
        private float pitch;
        private Transform cameraPivot;
        private Vector2 cameraMouseDelta;

        // Private Services
        private InputService inputService;
        private PlayerService playerService;

        public CameraService(CameraConfig _cameraConfig, Camera _mainCamera)
        {
            // Setting Variables
            cameraConfig = _cameraConfig;
            mainCamera = _mainCamera;
        }

        public void Init(InputService _inputService, PlayerService _playerService)
        {
            // Setting Services
            inputService = _inputService;
            playerService = _playerService;

            // Setting Elements
            yaw = cameraConfig.initialYaw;
            pitch = cameraConfig.initialPitch;
            cameraMouseDelta = Vector2.zero;

            AssignInputs();
        }

        public void Init2()
        {
            cameraPivot = playerService.GetController().GetCameraPivot();
        }

        private void AssignInputs()
        {
            // Camera Inputs
            InputControls inputControls = inputService.GetInputControls();

            inputControls.Camera.MouseDelta.performed += ctx => cameraMouseDelta = ctx.ReadValue<Vector2>();
            inputControls.Camera.MouseDelta.canceled += ctx => cameraMouseDelta = Vector2.zero;
        }

        public void Update() => HandleCameraRotation();

        public void LateUpdate() => HandleCameraPosition();

        private void HandleCameraRotation()
        {
            // Setting horizontal and vertical rotation based on mouse movement after clamping it on vertical axis
            yaw += cameraMouseDelta.x * cameraConfig.mouseSensitivity;
            pitch -= cameraMouseDelta.y * cameraConfig.mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -cameraConfig.minPitch, cameraConfig.maxPitch);

            cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        private void HandleCameraPosition()
        {
            // Setting Camera Position based on camera pivot on player
            Vector3 desiredPos = cameraPivot.position - cameraPivot.forward * cameraConfig.defaultDistance;

            // If Camera collides with a world object like ground, it will stop there, other wise the desired position
            if (Physics.SphereCast(cameraPivot.position, cameraConfig.cameraCollisionRadius,
                -cameraPivot.forward, out RaycastHit hit, cameraConfig.defaultDistance,
                cameraConfig.collisionLayers))
            {
                mainCamera.transform.position = hit.point + cameraPivot.forward * cameraConfig.cameraCollisionRadius;
            }
            else
            {
                mainCamera.transform.position = desiredPos;
            }

            // So that camera look towards the player, not just rotate orbitally
            mainCamera.transform.LookAt(cameraPivot.position);
        }

        // Getters
        public Transform GetTransform() => mainCamera.transform;
        public Vector3 GetCameraForwardXZNormalized() => GetCameraXZNormalizedVector(mainCamera.transform.forward);
        public Vector3 GetCameraRightXZNormalized() => GetCameraXZNormalizedVector(mainCamera.transform.right);
        private Vector3 GetCameraXZNormalizedVector(Vector3 _cameraVector)
        {
            // Disabling Camera Height as for movement of player, camera's height doesn't matter
            _cameraVector.y = 0;

            // Normalizing Camera Vectors as magnitude is not needed only direction
            _cameraVector.Normalize();

            return _cameraVector;
        }
    }
}