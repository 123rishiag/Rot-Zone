using Game.Controls;
using Game.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraService
    {
        private CameraConfig cameraConfig;
        private CinemachineCamera cmCamera;

        private InputService inputService;
        private PlayerService playerService;

        private CinemachineTargetGroup cinemachineTargetGroup;
        private CinemachineThirdPersonFollow cinemachineThirdPersonFollow;
        private CinemachineRotationComposer cinemachineRotationComposer;
        private CinemachineThirdPersonAim cinemachineThirdPersonAim;

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

            cinemachineThirdPersonFollow = cmCamera.GetComponent<CinemachineThirdPersonFollow>();
            if (cinemachineThirdPersonFollow == null)
            {
                Debug.LogError("Cinemachine Third Person Follow not found!!!");
            }
            cinemachineRotationComposer = cmCamera.GetComponent<CinemachineRotationComposer>();
            if (cinemachineRotationComposer == null)
            {
                Debug.LogError("Cinemachine Rotation Composer not found!!!");
            }
            cinemachineThirdPersonAim = cmCamera.GetComponent<CinemachineThirdPersonAim>();
            if (cinemachineThirdPersonAim == null)
            {
                Debug.LogError("Cinemachine Third Person Aim not found!!!");
            }
        }

        private void AssignInputs()
        {
            InputControls inputControls = inputService.GetInputControls();
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
        }

        // Setters
        public void SetCameraFOV(int _fov) => cmCamera.Lens.FieldOfView = _fov;

        // Getters
        public Transform GetCameraTransform() => cmCamera.transform;
    }
}
