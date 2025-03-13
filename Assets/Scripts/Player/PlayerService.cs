using ServiceLocator.Controls;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;

namespace ServiceLocator.Player
{
    public class PlayerService
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
            // Setting Elements
            playerController = new PlayerController(playerConfig.playerData, playerConfig.playerPrefab,
                _inputService, _cameraService, _weaponService);
        }

        public void Update() => playerController.Update();

        // Getters
        public PlayerController GetController() => playerController;
    }
}