using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class ThirdPersonCameraController : CameraController
    {
        // Private Variables
        private float yaw;
        private float pitch;

        private Vector2 mouseDelta;

        public ThirdPersonCameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService) :
            base(_cameraData, _parentPanel,
                _inputService, _playerService)
        {
            // Setting Variables
            yaw = 0f;
            pitch = 0f;

            InputControls.Camera.MouseDelta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
            InputControls.Camera.MouseDelta.canceled += ctx => mouseDelta = Vector2.zero;
        }

        protected override void AimCamera()
        {
            float dx = mouseDelta.x;
            float dy = mouseDelta.y;

            yaw += dx * CameraModel.CameraSensitivity * Time.deltaTime;
            pitch -= dy * CameraModel.CameraSensitivity * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -10f, 35f);

            PlayerService.GetController().GetView().transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        private void RotatePlayer()
        {
            
        }

        // Getters
        public override Ray GetAimRay() => Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }
}