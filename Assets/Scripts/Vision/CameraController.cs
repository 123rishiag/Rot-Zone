using UnityEngine;

namespace Game.Vision
{
    public class CameraController
    {
        // Private Variables
        private CameraModel cameraModel;
        private CameraView cameraView;

        public CameraController(CameraData _cameraData, Transform _parentPanel, Transform _targetTransform)
        {
            // Setting Variables
            cameraModel = new CameraModel(_cameraData);
            cameraView = Object.Instantiate(_cameraData.cameraPrefab, _parentPanel).GetComponent<CameraView>();
            cameraView.Init(_targetTransform);
        }

        // Getters
        public CameraModel GetModel() => cameraModel;
        public CameraView GetView() => cameraView;
    }
}