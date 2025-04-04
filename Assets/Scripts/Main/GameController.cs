using ServiceLocator.Controls;
using ServiceLocator.Enemy;
using ServiceLocator.Event;
using ServiceLocator.Player;
using ServiceLocator.Projectile;
using ServiceLocator.Sound;
using ServiceLocator.Spawn;
using ServiceLocator.UI;
using ServiceLocator.Utility;
using ServiceLocator.Vision;
using ServiceLocator.Wave;
using ServiceLocator.Weapon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController : IStateOwner<GameController>
    {
        // Private Variables
        private GameService gameService;

        // Private Services
        private EventService eventService;
        private InputService inputService;
        private CameraService cameraService;
        private SoundService soundService;
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
            eventService = new EventService();
            inputService = new InputService();
            cameraService = new CameraService(gameService.cameraConfig, gameService.mainCamera);
            soundService = new SoundService(gameService.soundConfig, gameService.bgmSource, gameService.sfxSource);
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
            // Event Service
            inputService.Init();
            cameraService.Init(inputService, playerService);
            soundService.Init(eventService);
            uiService.Init(eventService);
            projectileService.Init();
            weaponService.Init(eventService, projectileService);
            playerService.Init(eventService, inputService, cameraService, weaponService);
            enemyService.Init(eventService, playerService);
            spawnService.Init(playerService, enemyService);
            waveService.Init(eventService, inputService, spawnService, playerService, enemyService);
        }
        private void CreateStateMachine()
        {
            Owner = this;
            gameStateMachine = new GameStateMachine(this);
        }

        public void Reset()
        {
            // Event Service
            // Input Service
            cameraService.Reset();
            soundService.Reset();
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
            // Event Service
            inputService.Destroy();
            // Camera Service
            // Sound Service
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
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
            gameStateMachine.ChangeState(GameState.GAME_PLAY);
        }
        public void RestartGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            gameStateMachine.ChangeState(GameState.GAME_RESTART);
        }
        public void ControlMenu()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
            gameStateMachine.ChangeState(GameState.GAME_CONTROL);
        }
        public void MainMenu()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            SceneManager.LoadScene(0); // Reload 0th scene
        }
        public void QuitGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            Application.Quit();
        }

        public void MuteGame()
        {
            soundService.MuteGame(); // Mute/unmute the game
            uiService.GetController().SetMuteButtonText(soundService.IsMute);
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
        }

        // Getters
        public EventService GetEventService() => eventService;
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
