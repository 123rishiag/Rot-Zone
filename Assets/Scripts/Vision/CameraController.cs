using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public abstract class CameraController
    {
        // Private Variables
        protected CameraModel CameraModel;
        protected CameraView CameraView;

        protected InputControls InputControls;

        // Private Services
        private InputService inputService;
        protected PlayerService PlayerService;

        public CameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService)
        {
            // Setting Variables
            CameraModel = new CameraModel(_cameraData);
            CameraView = Object.Instantiate(_cameraData.cameraPrefab, _parentPanel).GetComponent<CameraView>();
            CameraView.Init(_playerService.GetController().GetTransform());

            // Setting Services
            inputService = _inputService;

            // Setting Elements
            InputControls = inputService.GetInputControls();
            PlayerService = _playerService;
        }

        public void Update() => AimCamera();

        protected abstract void AimCamera();

        public abstract Ray GetAimRay();

        // Getters
        public CameraModel GetModel() => CameraModel;
        public CameraView GetView() => CameraView;
    }
}