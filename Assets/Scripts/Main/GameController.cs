using ServiceLocator.Controls;
using ServiceLocator.Enemy;
using ServiceLocator.Player;
using ServiceLocator.Projectile;
using ServiceLocator.Spawn;
using ServiceLocator.UI;
using ServiceLocator.Utility;
using ServiceLocator.Vision;
using ServiceLocator.Wave;
using ServiceLocator.Weapon;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController : IStateOwner<GameController>
    {
        // Private Variables
        private GameService gameService;

        // Private Services
        private InputService inputService;
        private CameraService cameraService;
        private UIService uiService;
        private ProjectileService projectileService;
        private WeaponService weaponService;
        private PlayerService playerService;
        private EnemyService enemyService;
        private SpawnService spawnService;
        private WaveService waveService;

        public GameController Owner { get; set; }
        private GameStateMachine gameStateMachine;
        public bool IsPausePressed { get; set; }
        public GameController(GameService _gameService)
        {
            // Setting Variables
            gameService = _gameService;

            // Setting Services
            CreateServices();
            InjectDependencies();
            CreateStateMachine();

            // Setting Elements
            gameStateMachine.ChangeState(GameState.GAME_START);
            IsPausePressed = false;

            AssignInputs();
        }

        private void AssignInputs()
        {
            inputService.GetInputControls().Game.Pause.started += ctx => IsPausePressed = true;
            inputService.GetInputControls().Game.Pause.canceled += ctx => IsPausePressed = false;
        }

        private void CreateServices()
        {
            inputService = new InputService();
            cameraService = new CameraService(gameService.cameraConfig, gameService.mainCamera);
            uiService = new UIService(gameService.uiCanvas, this);
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
            uiService.Init();
            projectileService.Init();
            weaponService.Init(projectileService);
            playerService.Init(inputService, cameraService, weaponService);
            enemyService.Init(playerService);
            spawnService.Init(playerService, enemyService);
            waveService.Init(spawnService, enemyService);
        }
        private void CreateStateMachine()
        {
            Owner = this;
            gameStateMachine = new GameStateMachine(this);
        }

        public void Reset()
        {
            // Input Service
            cameraService.Reset();
            uiService.Reset();
            projectileService.Reset();
            // Weapon Service
            playerService.Reset();
            enemyService.Reset();
            // Spawn Service
            waveService.Reset();
        }
        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            uiService.Destroy();
            // Projectile Service
            // Weapon Service
            // Player Service
            // Enemy Service
            // Spawn Service
            // Wave Service
        }
        public void Update() => gameStateMachine.Update();
        public void LateUpdate() => gameStateMachine.LateUpdate();

        public void PlayGame()
        {
            gameStateMachine.ChangeState(GameState.GAME_PLAY);
        }
        public void RestartGame()
        {
            gameStateMachine.ChangeState(GameState.GAME_RESTART);
        }
        public void MainMenu()
        {
            SceneManager.LoadScene(0); // Reload 0th scene
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

        public void MuteGame()
        {
            //uiService.GetUIController().SetMuteButtonText(soundService.IsMute);
        }

        // Getters

        public InputService GetInputService() => inputService;
        public CameraService GetCameraService() => cameraService;
        public UIService GetUIService() => uiService;
        public ProjectileService GetProjectileService() => projectileService;
        public WeaponService GetWeaponService() => weaponService;
        public PlayerService GetPlayerService() => playerService;
        public EnemyService GetEnemyService() => enemyService;
        public SpawnService GetSpawnService() => spawnService;
        public WaveService GetWaveService() => waveService;

    }
}
