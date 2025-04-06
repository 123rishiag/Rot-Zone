using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class CameraService
    {
        // Private Variables
        private CameraConfig cameraConfig;
        private Camera mainCamera;

        private float yaw;
        private float pitch;
        private Vector2 cameraMouseDelta;
        private Quaternion cameraRotation;

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

            // Setting Variables
            yaw = cameraConfig.initialYaw;
            pitch = cameraConfig.initialPitch;
            cameraMouseDelta = Vector2.zero;
            cameraRotation = Quaternion.identity;

            AssignInputs();
            Reset();
        }

        private void AssignInputs()
        {
            // Camera Inputs
            InputControls inputControls = inputService.GetInputControls();

            inputControls.Camera.MouseDelta.performed += ctx => cameraMouseDelta = ctx.ReadValue<Vector2>();
            inputControls.Camera.MouseDelta.canceled += ctx => cameraMouseDelta = Vector2.zero;
        }

        public void Reset()
        {
            mainCamera.transform.position = Vector3.zero + Vector3.up * cameraConfig.cameraHeightOffset
                - Vector3.forward * cameraConfig.cameraDistanceOffset;
            mainCamera.transform.rotation = Quaternion.identity;
        }

        public void LateUpdate()
        {
            HandleCameraRotation();
            HandleCameraPosition();
        }

        private void HandleCameraRotation()
        {
            // Setting horizontal and vertical rotation based on mouse movement after clamping it on vertical axis
            yaw += cameraMouseDelta.x * cameraConfig.mouseSensitivity;
            pitch -= cameraMouseDelta.y * cameraConfig.mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -cameraConfig.minPitch, cameraConfig.maxPitch);

            cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        private void HandleCameraPosition()
        {
            Transform playerTransform = playerService.GetController().GetTransform();

            // Calculating player's target position
            Vector3 playerTargetPosition = playerTransform.position +
                Vector3.up * cameraConfig.cameraHeightOffset;

            // Side offset using camera's right vector
            Vector3 sideOffset = cameraRotation * Vector3.right * cameraConfig.cameraSideOffset;

            // Computing the camera's orbiting position
            Vector3 targetPosition = playerTargetPosition -
                cameraRotation * Vector3.forward * cameraConfig.cameraDistanceOffset + sideOffset;

            mainCamera.transform.position = targetPosition;

            // So that camera always look at player, not just orbit around without looking at it
            mainCamera.transform.LookAt(playerTransform.position + Vector3.up * cameraConfig.cameraHeightOffset);
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