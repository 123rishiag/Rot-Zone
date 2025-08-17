using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraView : MonoBehaviour
    {
        public void Init(Transform _followTransform, Transform _lookAtTransform)
        {
            CmCamera = GetComponentInChildren<CinemachineCamera>();
            if (CmCamera == null)
            {
                Debug.LogError("Cinemachine Camera not found!!!");
            }

            // Setting Camera Targets
            CmCamera.Follow = _followTransform;
            CmCamera.LookAt = _lookAtTransform;
        }

        // Getters
        public CinemachineCamera CmCamera { get; private set; }

    }
}