using ServiceLocator.Spawn;
using System;

namespace ServiceLocator.Wave
{
    public class WaveService
    {
        // Private Variables
        private WaveConfig waveConfig;
        private int currentWaveIndex;
        private WaveType currentWaveType;

        // Private Services
        private SpawnService spawnService;

        public WaveService(WaveConfig _waveConfig)
        {
            // Setting Variables
            waveConfig = _waveConfig;
            currentWaveIndex = 0;
            currentWaveType = waveConfig.waveData[0].waveType;
        }

        public void Init(SpawnService _spawnService)
        {
            // Setting Services
            spawnService = _spawnService;

            // Setting Elements
            LoadWave(currentWaveType);
        }

        private void LoadWave(WaveType _waveType)
        {
            WaveData waveData = GetWaveData(_waveType);
            spawnService.Spawn<PlayerSpawnData>(SpawnEntityType.PLAYER, new PlayerSpawnData[] { waveData.playerSpawnData });
            spawnService.Spawn<EnemySpawnData>(SpawnEntityType.ENEMY, waveData.enemySpawnDatas);
        }
        public void SetNextWave()
        {
            currentWaveIndex++;
            currentWaveType = waveConfig.waveData[currentWaveIndex].waveType;
            LoadWave(currentWaveType);
        }

        // Getters
        public bool IsLastWave() => currentWaveIndex >= waveConfig.waveData.Length - 1;
        private int GetCurrentWaveIndex() => currentWaveIndex;
        private WaveData GetWaveData(WaveType _waveType) =>
            Array.Find(waveConfig.waveData, w => w.waveType == _waveType);
    }
}
