using Game.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraService
    {
        private CameraConfig cameraConfig;
        private CinemachineCamera cmCamera;

        private PlayerService playerService;

        private CinemachinePositionComposer positionComposer;

        public CameraService(CameraConfig _cameraConfig, CinemachineCamera _cinemachineCamera)
        {
            cameraConfig = _cameraConfig;
            cmCamera = _cinemachineCamera;
        }

        public void Init(PlayerService _playerService)
        {
            playerService = _playerService;

            InitializeVariables();
            Reset();
        }

        private void InitializeVariables()
        {
            positionComposer = cmCamera.GetComponent<CinemachinePositionComposer>();
            if (positionComposer == null)
            {
                Debug.LogError("Position Composer not found!!!");
            }
        }

        public void Reset()
        {
            SetupCinemachineCamera();
        }

        private void SetupCinemachineCamera()
        {
            // Setting Camera Targets
            Transform playerTransform = playerService.GetController().GetTransform();
            cmCamera.Follow = playerTransform;
            //cmCamera.LookAt = playerTransform;

            // Setting Camera Rotation
            cmCamera.transform.rotation = Quaternion.Euler(
                cameraConfig.isometricPitch,
                0f,
                0f
            );

            // Setting Position Composer Settigns
            positionComposer.TargetOffset = Vector3.up * cameraConfig.cameraHeightOffset;
            positionComposer.CameraDistance = cameraConfig.cameraDistanceOffset;
            positionComposer.Damping =
                new Vector3(cameraConfig.cameraDamping, cameraConfig.cameraDamping, cameraConfig.cameraDamping);
        }
    }
}
