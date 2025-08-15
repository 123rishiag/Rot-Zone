using Game.Controls;
using Game.Player;
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
            cameraControllers = new List<CameraController>();

            InitializeVariables();
            CreateControllers();
            SetStateCameras();
            AssignInputs();

            Reset();
        }

        public void Reset()
        {

        }

        private void InitializeVariables()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Cinemachine Brain not found!!!");
            }
            cmBrain = mainCamera.GetComponent<CinemachineBrain>();

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
        private void SetStateCameras()
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
        private void AssignInputs()
        {
            InputControls inputControls = inputService.GetInputControls();
        }

        // Getters
        public Transform GetCurrentCameraTransform() => cmCamera.transform;
    }
}
