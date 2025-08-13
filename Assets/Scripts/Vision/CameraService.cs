using Game.Controls;
using Game.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Vision
{
    public class CameraService
    {
        private CameraConfig cameraConfig;
        private CinemachineCamera cmCamera;

        private InputService inputService;
        private PlayerService playerService;

        private CinemachineTargetGroup cinemachineTargetGroup;
        private CinemachineOrbitalFollow cinemachineOrbitalFollow;
        private CinemachineRotationComposer cinemachineRotationComposer;
        private CinemachineInputAxisController cinemachineInputAxisController;
        private CinemachineRecomposer cinemachineRecomposer;
        private CinemachineDecollider cinemachineDecollider;

        private Vector2 mouseDelta;

        public CameraService(CameraConfig _cameraConfig, CinemachineCamera _cinemachineCamera)
        {
            cameraConfig = _cameraConfig;
            cmCamera = _cinemachineCamera;
        }

        public void Init(InputService _inputService, PlayerService _playerService)
        {
            inputService = _inputService;
            playerService = _playerService;

            InitializeVariables();
            AssignInputs();

            Reset();
        }

        private void InitializeVariables()
        {
            cinemachineTargetGroup = cmCamera.GetComponentInChildren<CinemachineTargetGroup>();
            if (cinemachineTargetGroup == null)
            {
                Debug.LogError("Tracking Target Group not found!!!");
            }

            cinemachineOrbitalFollow = cmCamera.GetComponent<CinemachineOrbitalFollow>();
            if (cinemachineOrbitalFollow == null)
            {
                Debug.LogError("Cinemachine Orbital Person follow not found!!!");
            }
            cinemachineRotationComposer = cmCamera.GetComponent<CinemachineRotationComposer>();
            if (cinemachineRotationComposer == null)
            {
                Debug.LogError("Cinemachine Rotation Composer not found!!!");
            }
            cinemachineInputAxisController = cmCamera.GetComponent<CinemachineInputAxisController>();
            if (cinemachineInputAxisController == null)
            {
                Debug.LogError("Cinemachine Input Axis Controller not found!!!");
            }
            cinemachineRecomposer = cmCamera.GetComponent<CinemachineRecomposer>();
            if (cinemachineRecomposer == null)
            {
                Debug.LogError("Cinemachine Recomposer not found!!!");
            }
            cinemachineDecollider = cmCamera.GetComponent<CinemachineDecollider>();
            if (cinemachineDecollider == null)
            {
                Debug.LogError("Cinemachine Decollider not found!!!");
            }

            mouseDelta = Vector2.zero;
        }

        private void AssignInputs()
        {
            InputControls inputControls = inputService.GetInputControls();

            inputControls.Camera.MouseDelta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
            inputControls.Camera.MouseDelta.canceled += ctx => mouseDelta = Vector2.zero;
        }

        public void Reset()
        {
            SetupCinemachineCamera();
        }

        private void SetupCinemachineCamera()
        {
            InputControls inputControls = inputService.GetInputControls();

            // Setting Camera Targets
            Transform playerTransform = playerService.GetController().GetTransform();
            Transform aimTransform = playerService.GetController().GetAimTransform();
            cinemachineTargetGroup.Targets.Clear();
            cinemachineTargetGroup.AddMember(playerTransform,
                1f, 1f);
            cmCamera.Follow = cinemachineTargetGroup.transform;

            // Setting Input Axis Controller Settings for Camera
            var lookRef = InputActionReference.Create(inputControls.Camera.MouseDelta);

            var lookOrbitXController = cinemachineInputAxisController.Controllers[0];
            lookOrbitXController.Enabled = true;
            lookOrbitXController.Input.InputAction = lookRef;
            lookOrbitXController.Input.Gain = cameraConfig.mouseSensitivity;

            var lookOrbitYController = cinemachineInputAxisController.Controllers[1];
            lookOrbitYController.Enabled = true;
            lookOrbitYController.Input.InputAction = lookRef;
            lookOrbitYController.Input.Gain = -cameraConfig.mouseSensitivity;
        }

        // Getters
        public Transform GetCameraTransform() => cmCamera.transform;
    }
}
