using Game.Controls;
using Game.Player;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Vision
{
    public class CameraService
    {
        // Private Variables
        private CameraConfig cameraConfig;
        private CinemachineStateDrivenCamera cmCamera;

        private Camera mainCamera;
        private CinemachineBrain cmBrain;

        private Animator cmCameraAnimator;

        private List<CameraController> cameraControllers;
        private CameraController currentCameraController;

        // Private Services
        private InputService inputService;
        private PlayerService playerService;

        public CameraService(CameraConfig _cameraConfig, CinemachineStateDrivenCamera _cinemachineCamera)
        {
            // Setting Variables
            cameraConfig = _cameraConfig;
            cmCamera = _cinemachineCamera;
        }

        public void Init(InputService _inputService, PlayerService _playerService)
        {
            // Setting Services
            inputService = _inputService;
            playerService = _playerService;

            // Setting Elements
            cameraControllers = new List<CameraController>();

            AssignInputs();
            InitializeVariables();
            CreateControllers();
            AddCameraStates();

            SetCamera(CameraType.DEFAULT);

            Reset();
        }

        public void Reset()
        {

        }

        public void Update()
        {
            currentCameraController.Update();
        }

        private void AssignInputs()
        {
            InputControls inputControls = inputService.GetInputControls();

            inputControls.Camera.SwitchCamera.started += ctx => SwitchCamera();
        }
        private void InitializeVariables()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found!!!");
            }
            cmBrain = mainCamera.GetComponent<CinemachineBrain>();
            if (cmBrain == null)
            {
                Debug.LogError("Cinemachine Brain not found!!!");
            }
            cmCameraAnimator = cmCamera.GetComponent<Animator>();
            if (cmCameraAnimator == null)
            {
                Debug.LogError("Cinemachine Animator not found!!!");
            }

            Transform playerTransform = playerService.GetController().GetTransform();
            cmCamera.Follow = playerTransform;
        }

        private void CreateControllers()
        {
            Transform playerTransform = playerService.GetController().GetTransform();
            foreach (var cameraData in cameraConfig.cameraDatas)
            {
                CameraController cameraController;
                switch (cameraData.cameraType)
                {
                    case CameraType.THIRD_PERSON:
                        cameraController = new ThirdPersonCameraController(cameraData, cmCamera.transform,
                            inputService, playerService);
                        break;
                    case CameraType.ISOMETRIC:
                        cameraController = new IsometricCameraController(cameraData, cmCamera.transform,
                            inputService, playerService);
                        break;
                    case CameraType.DEFAULT:
                        cameraController = new DefaultCameraController(cameraData, cmCamera.transform,
                            inputService, playerService);
                        break;
                    default:
                        cameraController = new DefaultCameraController(cameraData, cmCamera.transform,
                            inputService, playerService);
                        break;
                }
                cameraControllers.Add(cameraController);
            }
        }
        private void AddCameraStates()
        {
            var instructionList = new List<CinemachineStateDrivenCamera.Instruction>(cmCamera.Instructions);

            // Setting Camera States
            foreach (var cameraController in cameraControllers)
            {
                int fullHash = (cameraController.GetModel().CameraType == CameraType.DEFAULT) ? 0 :
                    Animator.StringToHash("Base Layer." + cameraController.GetModel().CameraAnimationClip.name);

                var instruction = new CinemachineStateDrivenCamera.Instruction
                {
                    FullHash = fullHash,
                    Camera = cameraController.GetView().CmCamera,
                    ActivateAfter = 0f,
                    MinDuration = 0f
                };
                instructionList.Add(instruction);
            }

            // Adding all States
            cmCamera.Instructions = instructionList.ToArray();
        }

        private void SwitchCamera()
        {
            CameraType currentCameraType = currentCameraController.GetModel().CameraType;
            int currentIndex = Array.IndexOf(Enum.GetValues(typeof(CameraType)), currentCameraType);
            int maxLength = Enum.GetValues(typeof(CameraType)).Length;
            int nextIndex = (currentIndex + 1) % maxLength;

            CameraType cameraType = (CameraType)Enum.GetValues(typeof(CameraType)).GetValue(nextIndex);

            SetCamera(cameraType);
        }

        // Setters
        public void SetCamera(CameraType _cameraType)
        {
            currentCameraController = cameraControllers.Find(controller => controller.GetModel().CameraType == _cameraType);
            cmCameraAnimator.Play(currentCameraController.GetModel().CameraAnimationClip.name);
        }

        // Getters
        public Transform GetCurrentCameraTransform() => GetCurrentCameraController().GetView().gameObject.transform;
        public CameraController GetCurrentCameraController() => currentCameraController;

    }
}
