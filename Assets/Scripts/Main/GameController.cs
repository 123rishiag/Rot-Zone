using ServiceLocator.Controls;
using ServiceLocator.Player;
using ServiceLocator.Vision;
using ServiceLocator.Weapon;

namespace ServiceLocator.Main
{
    public class GameController
    {
        // Private Variables
        private GameService gameService;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;
        private WeaponService weaponService;
        private PlayerService playerService;

        public GameController(GameService _gameService)
        {
            // Setting Variables
            gameService = _gameService;

            // Setting Services
            CreateServices();
            InjectDependencies();
        }

        private void CreateServices()
        {
            inputService = new InputService();
            cameraService = new CameraService(gameService.cameraConfig, gameService.mainCamera);
            weaponService = new WeaponService(gameService.weaponConfig);
            playerService = new PlayerService(gameService.playerConfig);
        }

        private void InjectDependencies()
        {
            inputService.Init();
            cameraService.Init(inputService, playerService);
            // Weapon Service
            playerService.Init(inputService, cameraService, weaponService);
        }

        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            // Weapon Service
            // Player Service
        }

        public void Update()
        {
            // Input Service
            cameraService.Update();
            // Weapon Service
            playerService.Update();
        }

        public void LateUpdate()
        {
            // Input Service
            cameraService.LateUpdate();
            // Weapon Service
            // Player Service
        }
    }
}
