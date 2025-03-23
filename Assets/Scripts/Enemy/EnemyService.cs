using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyService
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

            CreateEnemy(EnemyType.SLOW_ZOMBIE, new Vector3(0f, 0f, -5f));
        }

        public void Update()
        {
            DestroyEnemys();
        }
        private void DestroyEnemys()
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

        public EnemyController CreateEnemy(EnemyType _enemyType, Vector3 _spawnPosition)
        {
            switch (_enemyType)
            {
                case EnemyType.SLOW_ZOMBIE:
                    return enemyPool.GetEnemy<EnemyController>(_enemyType, _spawnPosition);
                default:
                    Debug.LogWarning($"Unhandled EnemyType: {_enemyType}");
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