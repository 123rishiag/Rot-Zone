using ServiceLocator.Enemy;
using ServiceLocator.Spawn;
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
        private SpawnService spawnService;
        private EnemyService enemyService;

        public WaveService(WaveConfig _waveConfig)
        {
            // Setting Variables
            waveConfig = _waveConfig;
        }

        public void Init(SpawnService _spawnService, EnemyService _enemyService)
        {
            // Setting Variables
            currentWaveIndex = 0;
            CurrentWaveType = waveConfig.waveData[0].waveType;
            IsLastWaveComplete = false;

            // Setting Services
            spawnService = _spawnService;
            enemyService = _enemyService;

            // Setting Elements
            CreateStateMachine();
            waveStateMachine.ChangeState(WaveState.START);
        }

        private void CreateStateMachine()
        {
            Owner = this;
            waveStateMachine = new WaveStateMachine(this);
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
        public bool IsWaveComplete() => !enemyService.IsAnyEnemyAlive();
        private WaveData GetWaveData(WaveType _waveType) =>
            Array.Find(waveConfig.waveData, w => w.waveType == _waveType);
    }
}
