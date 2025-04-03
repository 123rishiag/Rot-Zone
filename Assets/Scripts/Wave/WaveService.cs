using ServiceLocator.Controls;
using ServiceLocator.Enemy;
using ServiceLocator.Player;
using ServiceLocator.Spawn;
using ServiceLocator.UI;
using ServiceLocator.Utility;
using System;

namespace ServiceLocator.Wave
{
    public class WaveService : IStateOwner<WaveService>
    {
        // Private Variables
        private WaveConfig waveConfig;
        private int currentWaveIndex;
        public WaveType CurrentWaveType { get; private set; }
        public bool IsLastWaveComplete { get; set; }
        public WaveService Owner { get; set; }
        private WaveStateMachine waveStateMachine;

        // Private Services
        public InputService InputService { get; private set; }
        private SpawnService spawnService;
        public PlayerService PlayerService { get; private set; }
        private EnemyService enemyService;
        public UIService UIService { get; private set; }

        public WaveService(WaveConfig _waveConfig)
        {
            // Setting Variables
            waveConfig = _waveConfig;
        }

        public void Init(
            InputService _inputService, SpawnService _spawnService, PlayerService _playerService, EnemyService _enemyService,
            UIService _uiService)
        {
            // Setting Services
            InputService = _inputService;
            spawnService = _spawnService;
            PlayerService = _playerService;
            enemyService = _enemyService;
            UIService = _uiService;

            // Setting Elements
            CreateStateMachine();
            Reset();
        }

        private void CreateStateMachine()
        {
            Owner = this;
            waveStateMachine = new WaveStateMachine(this);
        }

        public void Reset()
        {
            currentWaveIndex = 0;
            CurrentWaveType = waveConfig.waveData[0].waveType;
            IsLastWaveComplete = false;
            waveStateMachine.ChangeState(WaveState.START);
        }

        public void Update()
        {
            waveStateMachine.Update();
        }

        public void LoadCurrentWave()
        {
            WaveData waveData = GetWaveData(CurrentWaveType);
            spawnService.Spawn<PlayerSpawnData>(SpawnEntityType.PLAYER, new PlayerSpawnData[] { waveData.playerSpawnData });
            spawnService.Spawn<EnemySpawnData>(SpawnEntityType.ENEMY, waveData.enemySpawnDatas);
        }

        // Setters
        public void SetNextWave()
        {
            currentWaveIndex++;
            CurrentWaveType = waveConfig.waveData[currentWaveIndex].waveType;
        }

        // Getters
        public bool IsLastWave() => currentWaveIndex >= waveConfig.waveData.Length - 1;
        public bool IsWaveComplete() => enemyService.EnemiesAliveCount() == 0;
        public WaveStateMachine GetWaveStateMachine() => waveStateMachine;
        private WaveData GetWaveData(WaveType _waveType) =>
            Array.Find(waveConfig.waveData, w => w.waveType == _waveType);
    }
}
