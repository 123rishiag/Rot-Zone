using UnityEngine;

namespace Game.Vision
{
    public class CameraModel
    {
        public CameraModel(CameraData _cameraData)
        {
            CameraType = _cameraData.cameraType;
            CameraAnimationClip = _cameraData.cameraAnimationClip;
        }

        // Getters
        public CameraType CameraType { get; private set; }
        public AnimationClip CameraAnimationClip { get; private set; }
    }
}