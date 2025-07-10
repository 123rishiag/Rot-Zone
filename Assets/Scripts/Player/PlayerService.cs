using Game.Controls;
using Game.Event;
using Game.Spawn;
using Game.Vision;
using Game.Wave;
using Game.Weapon;
using System;
using UnityEngine;

namespace Game.Player
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

        public void Init(
            EventService _eventService, InputService _inputService, WeaponService _weaponService, CameraService _cameraService)
        {
            // Setting Variables
            playerController = new PlayerController(playerConfig.playerData, playerConfig.playerPrefab, Vector3.zero,
                _eventService, _inputService, _weaponService, _cameraService);
            Reset();
        }

        public void Reset()
        {
            playerController.GetMovementStateMachine().ChangeState(PlayerMovementState.IDLE);
            playerController.GetActionStateMachine().ChangeState(PlayerActionState.NONE);
            playerController.GetWeaponController().EquipWeapon(WeaponType.NONE);
            playerController.GetView().SetPosition(Vector3.zero);
        }

        public void Update() => playerController.Update();

        public void OnSpawn(Func<Vector3> _spawnPositionFunc, PlayerSpawnData _spawnData)
        {
            playerController.Reset(playerConfig.playerData, _spawnPositionFunc());

            playerController.SetHealth(_spawnData.playerHealth);

            // Adding Ammo for Weapons
            for (int i = 0; i < _spawnData.playerSpawnAmmoDatas.Length; ++i)
            {
                PlayerSpawnAmmoData playerSpawnAmmoData = _spawnData.playerSpawnAmmoDatas[i];
                playerController.GetWeaponController().SetAmmo(playerSpawnAmmoData.weaponType, 0);
                playerController.GetWeaponController().SetAmmo(playerSpawnAmmoData.weaponType, playerSpawnAmmoData.ammoToAdd);
            }
            playerController.UpdateUI();
        }

        // Getters
        public PlayerController GetController() => playerController;
        public bool IsPlayerAlive() => playerController.IsAlive;
    }
}