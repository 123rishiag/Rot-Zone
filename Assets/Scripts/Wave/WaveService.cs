using Game.Controls;
using Game.Enemy;
using Game.Event;
using Game.Player;
using Game.Spawn;
using System;

namespace Game.Wave
{
    public class WaveService
    {
        // Private Variables
        private WaveConfig waveConfig;
        private int currentWaveIndex;
        public WaveType CurrentWaveType { get; private set; }
        public bool IsLastWaveComplete { get; set; }

        private WaveStateMachine waveStateMachine;

        // Private Services
        public EventService EventService { get; private set; }
        public InputService InputService { get; private set; }
        private SpawnService spawnService;
        public PlayerService PlayerService { get; private set; }
        private EnemyService enemyService;

        public WaveService(WaveConfig _waveConfig)
        {
            // Setting Variables
            waveConfig = _waveConfig;
        }

        public void Init(EventService _eventService,
            InputService _inputService, SpawnService _spawnService, PlayerService _playerService, EnemyService _enemyService)
        {
            // Setting Services
            EventService = _eventService;
            InputService = _inputService;
            spawnService = _spawnService;
            PlayerService = _playerService;
            enemyService = _enemyService;

            // Setting Elements
            CreateStateMachine();
            Reset();
        }

        private void CreateStateMachine()
        {
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
