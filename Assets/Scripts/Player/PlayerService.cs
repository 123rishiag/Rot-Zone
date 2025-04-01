using ServiceLocator.Controls;
using ServiceLocator.Spawn;
using ServiceLocator.Vision;
using ServiceLocator.Wave;
using ServiceLocator.Weapon;
using System;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerService : ISpawn<PlayerSpawnData>
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

        public void OnSpawn(Func<Vector3> _spawnPositionFunc, PlayerSpawnData _spawnData)
        {
            playerController.Reset(playerConfig.playerData, _spawnPositionFunc());

            playerController.IncreaseHealth(_spawnData.playerHealth);

            // Adding Ammo for Weapons
            for (int i = 0; i < _spawnData.playerSpawnAmmoDatas.Length; ++i)
            {
                PlayerSpawnAmmoData playerSpawnAmmoData = _spawnData.playerSpawnAmmoDatas[i];
                playerController.GetWeaponController().SetAmmo(playerSpawnAmmoData.weaponType, 0);
                playerController.GetWeaponController().SetAmmo(playerSpawnAmmoData.weaponType, playerSpawnAmmoData.ammoToAdd);
            }
        }

        // Getters
        public PlayerController GetController() => playerController;
        public bool IsPlayerAlive() => playerController.IsAlive;
    }
}