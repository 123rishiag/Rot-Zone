using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class FirstPersonArmedCameraController : CameraController
    {
        public FirstPersonArmedCameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService) :
            base(_cameraData, _parentPanel,
                _inputService, _playerService)
        { }

        protected override void AimCamera()
        {
            base.AimCamera();

            PlayerService.GetController().RotatePlayerTowards(
                Quaternion.Euler(CameraView.CmCamera.LookAt.eulerAngles.x, Yaw, CameraView.CmCamera.LookAt.eulerAngles.z));
        }

        // Getters
        public override Transform GetCameraTransform() => CameraView.CmCamera.LookAt.transform;
    }
}