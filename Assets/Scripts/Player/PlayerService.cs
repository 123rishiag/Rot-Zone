using ServiceLocator.Controls;
using ServiceLocator.Spawn;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerService : ISpawn
    {
        // Private Variables
        private PlayerConfig playerConfig;
        private PlayerController playerController;

        public PlayerService(PlayerConfig _playerConfig)
        {
            // Setting Variables
            playerConfig = _playerConfig;
        }

        public void Init(InputService _inputService, CameraService _cameraService, WeaponService _weaponService)
        {
            // Setting Variables
            playerController = new PlayerController(playerConfig.playerData, playerConfig.playerPrefab, Vector3.zero,
                _inputService, _cameraService, _weaponService);
        }

        public void Update() => playerController.Update();

        public void OnSpawn(Vector3 _spawnPosition)
        {
            playerController.Reset(playerConfig.playerData, _spawnPosition);
        }

        // Getters
        public PlayerController GetController() => playerController;
    }
}