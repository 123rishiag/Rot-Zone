using ServiceLocator.Controls;
using ServiceLocator.Player;
using ServiceLocator.Projectile;
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
        private ProjectileService projectileService;
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
            projectileService = new ProjectileService(gameService.projectileConfig, gameService.projectilePoolPanel);
            weaponService = new WeaponService(gameService.weaponConfig);
            playerService = new PlayerService(gameService.playerConfig);
        }

        private void InjectDependencies()
        {
            inputService.Init();
            cameraService.Init(inputService, playerService);
            // Projectile Service
            weaponService.Init(projectileService);
            playerService.Init(inputService, cameraService, weaponService);
        }

        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            // Projectile Service
            // Weapon Service
            // Player Service
        }

        public void Update()
        {
            // Input Service
            cameraService.Update();
            // Projectile Service
            // Weapon Service
            playerService.Update();
        }

        public void LateUpdate()
        {
            // Input Service
            cameraService.LateUpdate();
            // Projectile Service
            // Weapon Service
            // Player Service
        }
    }
}
