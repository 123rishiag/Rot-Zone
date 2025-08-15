using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraView : MonoBehaviour
    {
        public void Init(Transform _targetTransform)
        {
            CmCamera = GetComponentInChildren<CinemachineCamera>();
            if (CmCamera == null)
            {
                Debug.LogError("Cinemachine Camera not found!!!");
            }

            // Setting Camera Targets
            CmCamera.Follow = _targetTransform;
        }

        // Getters
        public CinemachineCamera CmCamera { get; private set; }

    }
}