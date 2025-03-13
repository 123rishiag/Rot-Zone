using ServiceLocator.Controls;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerService
    {
        // Private Variables
        private PlayerConfig playerConfig;

        private PlayerController playerController;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;
        private WeaponService weaponService;

        public PlayerService(PlayerConfig _playerConfig)
        {
            // Setting Variables
            playerConfig = _playerConfig;
        }

        public void Init(InputService _inputService, CameraService _cameraService, WeaponService _weaponService)
        {
            // Setting Services
            inputService = _inputService;
            cameraService = _cameraService;
            weaponService = _weaponService;

            // Setting Elements
            playerController = Object.Instantiate(playerConfig.playerPrefab).GetComponent<PlayerController>();
            playerController.Init(playerConfig, inputService, cameraService, weaponService);
        }

        public void Update() => playerController.UpdateData();

        // Getters
        public PlayerController GetController() => playerController;
    }
}