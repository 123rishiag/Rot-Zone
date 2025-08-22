using UnityEngine;

namespace Game.Vision
{
    public class CameraModel
    {
        public CameraModel(CameraData _cameraData)
        {
            CameraType = _cameraData.cameraType;
            CameraAnimationClip = _cameraData.cameraAnimationClip;

            CameraSensitivity = _cameraData.cameraSensitivity;
            CameraPitchThresholdMin = _cameraData.cameraPitchThresholdMin;
            CameraPitchThresholdMax = _cameraData.cameraPitchThresholdMax;
            CameraDefaultFOV = _cameraData.cameraDefaultFOV;
            CameraZoomFOV = _cameraData.cameraZoomFOV;
        }

        // Getters
        public CameraType CameraType { get; private set; }
        public AnimationClip CameraAnimationClip { get; private set; }

        public int CameraSensitivity { get; private set; }
        public int CameraPitchThresholdMin { get; private set; }
        public int CameraPitchThresholdMax { get; private set; }
        public int CameraDefaultFOV { get; private set; }
        public int CameraZoomFOV { get; private set; }
    }
}