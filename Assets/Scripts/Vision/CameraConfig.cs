using UnityEngine;

namespace Game.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        public float isometricPitch = 45f;

        public float cameraDistanceOffset = 9f;
        public float cameraHeightOffset = 1f;

        public float cameraDamping = 0.75f;
    }
}