using System;
using UnityEngine;

namespace Game.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        public CameraData[] cameraDatas;
    }

    [Serializable]
    public class CameraData
    {
        public CameraType cameraType;
        public CameraView cameraPrefab;
        public AnimationClip cameraAnimationClip;
        public int cameraSensitivity = 20;
    }
}