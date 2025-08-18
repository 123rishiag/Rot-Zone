using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class DefaultCameraController : CameraController
    {
        public DefaultCameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService) :
            base(_cameraData, _parentPanel,
                _inputService, _playerService)
        { }

        protected override void AimCamera() { }

        // Getters
        public override Transform GetCameraTransform() => CameraView.CmCamera.Follow.transform;
    }
}