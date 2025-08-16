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
        public override Ray GetAimRay() => Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }
}