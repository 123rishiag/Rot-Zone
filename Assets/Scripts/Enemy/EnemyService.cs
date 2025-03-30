using ServiceLocator.Player;
using ServiceLocator.Spawn;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyService : ISpawn
    {
        // Private Variables
        private EnemyConfig enemyConfig;
        private Transform enemyParentPanel;

        private EnemyPool enemyPool;

        // Private Services
        private PlayerService playerService;

        public EnemyService(EnemyConfig _enemyConfig, Transform _parentPanel)
        {
            // Setting Variables
            enemyConfig = _enemyConfig;
            enemyParentPanel = _parentPanel;
        }

        public void Init(PlayerService _playerService)
        {
            // Setting Services
            playerService = _playerService;

            // Setting Elements
            enemyPool = new EnemyPool(enemyConfig, enemyParentPanel, playerService);
        }

        public void Update()
        {
            UpdateEnemies();
            DestroyEnemies();
        }
        private void UpdateEnemies()
        {
            for (int i = enemyPool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's isUsed is false
                if (!enemyPool.pooledItems[i].isUsed)
                {
                    continue;
                }

                var enemyController = enemyPool.pooledItems[i].Item;
                enemyController.Update();
            }
        }
        private void DestroyEnemies()
        {
            for (int i = enemyPool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's isUsed is false
                if (!enemyPool.pooledItems[i].isUsed)
                {
                    continue;
                }

                var enemyController = enemyPool.pooledItems[i].Item;
                if (!enemyController.IsActive())
                {
                    ReturnEnemyToPool(enemyController);
                }
            }
        }

        public void OnSpawn(Vector3 _spawnPosition)
        {
            int randomIndex = Random.Range(0, enemyConfig.enemyData.Length);
            EnemyType enemyType = enemyConfig.enemyData[randomIndex].enemyType;
            CreateEnemy(enemyType, _spawnPosition);
        }

        public EnemyController CreateEnemy(EnemyType _enemyType, Vector3 _spawnPosition)
        {
            switch (_enemyType)
            {
                case EnemyType.SLOW_ZOMBIE:
                    return enemyPool.GetEnemy<SlowEnemyController>(_enemyType, _spawnPosition);
                case EnemyType.FAST_ZOMBIE:
                    return enemyPool.GetEnemy<FastEnemyController>(_enemyType, _spawnPosition);
                default:
                    Debug.LogError($"Unhandled EnemyType: {_enemyType}");
                    return null;
            }
        }

        private void ReturnEnemyToPool(EnemyController _enemyToReturn)
        {
            _enemyToReturn.GetView().HideView();
            enemyPool.ReturnItem(_enemyToReturn);
        }
    }
}