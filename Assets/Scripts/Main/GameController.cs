using ServiceLocator.Controls;
using ServiceLocator.Enemy;
using ServiceLocator.Player;
using ServiceLocator.Projectile;
using ServiceLocator.Spawn;
using ServiceLocator.Vision;
using ServiceLocator.Wave;
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
        private SpawnService spawnService;
        private WaveService waveService;

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
            spawnService = new SpawnService(gameService.spawnTransformPanel);
            waveService = new WaveService(gameService.waveConfig);
        }

        private void InjectDependencies()
        {
            inputService.Init();
            cameraService.Init(inputService, playerService);
            projectileService.Init();
            weaponService.Init(projectileService);
            playerService.Init(inputService, cameraService, weaponService);
            enemyService.Init(playerService);
            spawnService.Init(playerService, enemyService);
            waveService.Init(spawnService);
        }

        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            // Projectile Service
            // Weapon Service
            // Player Service
            // Enemy Service
            // Spawn Service
            // Wave Service
        }

        public void Update()
        {
            // Input Service
            cameraService.Update();
            projectileService.Update();
            // Weapon Service
            playerService.Update();
            enemyService.Update();
            // Spawn Service
            // Wave Service
        }

        public void LateUpdate()
        {
            // Input Service
            cameraService.LateUpdate();
            // Projectile Service
            weaponService.LateUpdate();
            // Player Service
            // Enemy Service
            // Spawn Service
            // Wave Service
        }
    }
}
