using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class ThirdPersonUnarmedCameraController : CameraController
    {
        public ThirdPersonUnarmedCameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService) :
            base(_cameraData, _parentPanel,
                _inputService, _playerService)
        { }

        protected override void AimCamera()
        {
            base.AimCamera();

            PlayerController playerController = PlayerService.GetController();
            Vector3 moveDirection = playerController.GetXZNormalized(playerController.GetMoveDirection());
            if (moveDirection != Vector3.zero)
            {
                PlayerService.GetController().RotatePlayerTowards(Quaternion.LookRotation(moveDirection));
            }
        }

        // Getters
        public override Transform GetCameraTransform() => CameraView.CmCamera.Follow.transform;
    }
}