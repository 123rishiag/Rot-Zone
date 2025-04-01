using UnityEngine;

namespace ServiceLocator.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Camera Settings")]
        public float mouseSensitivity = 0.5f;
        public float cameraSideOffset = 0.5f;
        public float minPitch = 10f;
        public float maxPitch = 20f;
        public float cameraDistanceOffset = 4f;
        public float cameraHeightOffset = 1f;
        public float initialYaw = 0f;
        public float initialPitch = 20f;
    }
}