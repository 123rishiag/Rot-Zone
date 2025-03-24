using ServiceLocator.Controls;
using ServiceLocator.Enemy;
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
        private EnemyService enemyService;

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
            enemyService = new EnemyService(gameService.enemyConfig, gameService.enemyPoolPanel);
        }

        private void InjectDependencies()
        {
            inputService.Init();
            cameraService.Init(inputService, playerService);
            projectileService.Init();
            weaponService.Init(projectileService);
            playerService.Init(inputService, cameraService, weaponService);
            enemyService.Init(playerService);
        }

        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            // Projectile Service
            // Weapon Service
            // Player Service
            // Enemy Service
        }

        public void Update()
        {
            // Input Service
            cameraService.Update();
            projectileService.Update();
            // Weapon Service
            playerService.Update();
            enemyService.Update();
        }

        public void LateUpdate()
        {
            // Input Service
            cameraService.LateUpdate();
            // Projectile Service
            weaponService.LateUpdate();
            // Player Service
            // Enemy Service
        }
    }
}
