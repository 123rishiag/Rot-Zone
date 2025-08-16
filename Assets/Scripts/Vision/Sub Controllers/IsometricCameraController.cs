using Game.Controls;
using Game.Player;
using UnityEngine;

namespace Game.Vision
{
    public class IsometricCameraController : CameraController
    {
        // Private Variables
        private Vector2 mousePosition;
        private Vector2 aimPosition;

        public IsometricCameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService) :
            base(_cameraData, _parentPanel,
                _inputService, _playerService)
        {
            // Setting Variables
            InputControls.Camera.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
            InputControls.Camera.MousePosition.canceled += ctx => mousePosition = Vector2.zero;
        }

        protected override void AimCamera()
        {
            PlayerController playerController = PlayerService.GetController();
            PlayerView playerView = playerController.GetView();

            // Rotate Player Towards Camera, when player is not falling
            if (playerController.GetMovementStateMachine().GetCurrentState() == PlayerMovementState.FALL)
                return;

            aimPosition = ClampToCenterOffset(mousePosition);

            // Direction from player to aim Position
            Ray ray = GetAimRay();
            Plane ground = new Plane(Vector3.up, new Vector3(0f, playerView.transform.position.y, 0f));
            Vector3 worldPoint = Vector3.zero;
            if (ground.Raycast(ray, out float distance))
            {
                worldPoint = ray.GetPoint(distance);
            }
            Vector3 targetLocation = (worldPoint - playerView.transform.position);
            targetLocation.y = 0f;
            targetLocation.Normalize();

            // Current Player forward
            Vector3 playerForward = playerView.transform.forward;
            playerForward.y = 0f;
            playerForward.Normalize();

            float angle = Vector3.Angle(playerForward, targetLocation);
            if (angle < 25f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetLocation);
            PlayerService.GetController().GetView().transform.rotation = Quaternion.RotateTowards(
                playerView.transform.rotation, targetRotation, Time.deltaTime *
                CameraModel.CameraSensitivity);
        }
        private Vector2 ClampToCenterOffset(Vector2 _position)
        {
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 offset = _position - center;

            // Setting Clamp Ratio
            float maxX = Screen.width * 0.35f;
            float maxY = Screen.height * 0.35f;

            offset.x = Mathf.Clamp(offset.x, -maxX, maxX);
            offset.y = Mathf.Clamp(offset.y, -maxY, maxY);

            return center + offset;
        }

        // Getters
        public override Ray GetAimRay() => Camera.main.ScreenPointToRay(aimPosition);
    }
}