using UnityEngine;

namespace Game.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        public float cameraPitch = 45f; 
        public float cameraYaw = 45f;

        public float cameraDistanceOffset = 9f;
        public float cameraHeightOffset = 1f;

        public float cameraDamping = 0.75f;
    }
}