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

        protected float Yaw;
        protected float Pitch;

        private InputControls inputControls;
        private Vector2 mouseDelta;

        // Private Services
        protected InputService InputService;
        protected PlayerService PlayerService;

        public CameraController(CameraData _cameraData, Transform _parentPanel,
            InputService _inputService, PlayerService _playerService)
        {
            // Setting Variables
            CameraModel = new CameraModel(_cameraData);
            CameraView = Object.Instantiate(_cameraData.cameraPrefab, _parentPanel).GetComponent<CameraView>();
            CameraView.Init(_playerService.GetController().GetCameraPivotTransform(),
                _playerService.GetController().GetTransform());

            // Setting Services
            InputService = _inputService;
            PlayerService = _playerService;

            // Setting Elements
            inputControls = InputService.GetInputControls();

            Yaw = 0f;
            Pitch = 0f;

            inputControls.Camera.MouseDelta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
            inputControls.Camera.MouseDelta.canceled += ctx => mouseDelta = Vector2.zero;
        }

        public void Reset() => SetCameraDefaultFOV();

        public void Update() => AimCamera();

        protected virtual void AimCamera()
        {
            float dx = mouseDelta.x;
            float dy = mouseDelta.y;

            Yaw += dx * CameraModel.CameraSensitivity * Time.deltaTime;
            Pitch -= dy * CameraModel.CameraSensitivity * Time.deltaTime;

            Pitch = Mathf.Clamp(Pitch, -CameraModel.CameraPitchThresholdMin, CameraModel.CameraPitchThresholdMax);

            CameraView.CmCamera.Follow.rotation =
                Quaternion.Euler(Pitch, Yaw, CameraView.CmCamera.Follow.eulerAngles.z);
        }

        // Setters
        public void SetCameraDefaultFOV() => CameraView.CmCamera.Lens.FieldOfView = CameraModel.CameraDefaultFOV;
        public void SetCameraZoomFOV() => CameraView.CmCamera.Lens.FieldOfView = CameraModel.CameraZoomFOV;

        // Getters
        public CameraModel GetModel() => CameraModel;
        public CameraView GetView() => CameraView;
        public abstract Transform GetCameraTransform();
    }
}