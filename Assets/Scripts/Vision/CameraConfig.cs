using UnityEngine;

namespace Game.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Camera Setup Settings")]
        public float cameraPitch = 65f;
        public float cameraYaw = 180f;

        [Header("Camera Composition Settings")]
        public float cameraDistanceOffset = 9f;
        public float cameraDeadZoneDepth = 3f;
        [Space]
        public bool cameraDeadZoneEnabled = true;
        public Vector2 cameraDeadZoneSize = new Vector2(0.4f, 0.3f);
        public bool cameraHardLimitEnabled = true;
        public Vector2 cameraHardLimitSize = new Vector2(0.8f, 0.6f);
        [Space]
        public bool cameraCentreActivateEnabled = true;
        [Space]
        public float cameraHeightOffset = 0f;
        public float cameraDamping = 0.75f;
        [Space]
        public bool cameraLookAheadEnabled = true;
        [Range(0f, 1f)]
        public float lookAheadTimeFactor = 0.5f;
        [Range(0f, 30f)]
        public float lookAheadSmoothingRate = 20f;
        public bool lookAheadIgnoreY = true;
        [Space]
        public Vector2 playerCameraWeight = new Vector2(2f, 3f);
        public Vector2 aimCameraWeight = new Vector2(1f, 1f);
    }
}