using Game.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraService
    {
        private CameraConfig cameraConfig;
        private CinemachineCamera cmCamera;
        private CinemachineTargetGroup cinemachineTargetGroup;

        private PlayerService playerService;

        private CinemachinePositionComposer positionComposer;
        public Transform CameraTransform { get; private set; }

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
            cinemachineTargetGroup = cmCamera.GetComponentInChildren<CinemachineTargetGroup>();
            if (cinemachineTargetGroup == null)
            {
                Debug.LogError("Tracking Target Group not found!!!");
            }
            CameraTransform = cmCamera.transform;
        }

        public void Reset()
        {
            SetupCinemachineCamera();
        }

        private void SetupCinemachineCamera()
        {
            // Setting Camera Rotation
            cmCamera.transform.rotation = Quaternion.Euler(
                cameraConfig.cameraPitch,
                cameraConfig.cameraYaw,
                0f
            );

            // Setting Position Composer Settings
            positionComposer.CameraDistance = cameraConfig.cameraDistanceOffset;
            positionComposer.DeadZoneDepth = cameraConfig.cameraDeadZoneDepth;

            if (cameraConfig.cameraDeadZoneEnabled)
            {
                positionComposer.Composition.DeadZone.Enabled = true;
                positionComposer.Composition.DeadZone.Size = cameraConfig.cameraDeadZoneSize;
            }
            else
            {
                positionComposer.Composition.DeadZone.Enabled = false;
            }
            if (cameraConfig.cameraHardLimitEnabled)
            {
                positionComposer.Composition.HardLimits.Enabled = true;
                positionComposer.Composition.HardLimits.Size = cameraConfig.cameraHardLimitSize;
            }
            else
            {
                positionComposer.Composition.HardLimits.Enabled = false;
            }

            positionComposer.CenterOnActivate = cameraConfig.cameraCentreActivateEnabled;

            positionComposer.TargetOffset = Vector3.up * cameraConfig.cameraHeightOffset;
            positionComposer.Damping =
                new Vector3(cameraConfig.cameraDamping, cameraConfig.cameraDamping, cameraConfig.cameraDamping);

            positionComposer.Lookahead.Enabled = cameraConfig.cameraLookAheadEnabled;
            positionComposer.Lookahead.Time = cameraConfig.lookAheadTimeFactor;
            positionComposer.Lookahead.Smoothing = cameraConfig.lookAheadSmoothingRate;
            positionComposer.Lookahead.IgnoreY = cameraConfig.lookAheadIgnoreY;

            // Setting Camera Targets
            Transform playerTransform = playerService.GetController().GetTransform();
            Transform aimTransform = playerService.GetController().GetAimTransform();
            cinemachineTargetGroup.Targets.Clear();
            cinemachineTargetGroup.AddMember(playerTransform,
                cameraConfig.playerCameraWeight.y, cameraConfig.playerCameraWeight.x);
            cinemachineTargetGroup.AddMember(aimTransform,
                cameraConfig.aimCameraWeight.y, cameraConfig.aimCameraWeight.x);
            cmCamera.Follow = cinemachineTargetGroup.transform;
        }
    }
}
