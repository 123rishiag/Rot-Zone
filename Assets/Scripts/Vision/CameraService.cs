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

        private CameraType currentCameraType;
        private List<CameraController> cameraControllers;

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
            currentCameraType = CameraType.DEFAULT;
            cameraControllers = new List<CameraController>();

            AssignInputs();
            InitializeVariables();
            CreateControllers();
            AddCameraStates();

            Reset();
        }

        public void Reset()
        {

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
                CameraController cameraController = new CameraController(cameraData, cmCamera.transform, playerTransform);
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
            int currentIndex = Array.IndexOf(Enum.GetValues(typeof(CameraType)), currentCameraType);
            int maxLength = Enum.GetValues(typeof(CameraType)).Length;
            int nextIndex = (currentIndex + 1) % maxLength;

            CameraType cameraType = (CameraType)Enum.GetValues(typeof(CameraType)).GetValue(nextIndex);
            SetCamera(cameraType);
        }

        // Setters
        public void SetCamera(CameraType _cameraType)
        {
            currentCameraType = _cameraType;
            cmCameraAnimator.Play(GetCurrentCameraController().GetModel().CameraAnimationClip.name);
        }

        // Getters
        public Transform GetCurrentCameraTransform() => GetCurrentCameraController().GetView().gameObject.transform;
        public CameraController GetCurrentCameraController() =>
            cameraControllers.Find(controller => controller.GetModel().CameraType == currentCameraType);
    }
}
