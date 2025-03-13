using UnityEngine;

namespace ServiceLocator.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Camera Settings")]
        public float mouseSensitivity = 0.5f;
        public float minPitch = 20f;
        public float maxPitch = 60f;
        public float cameraDistanceOffset = 2.5f;
        public float cameraHeightOffset = 1f;
        public float initialYaw = 0f;
        public float initialPitch = 20f;
    }
}