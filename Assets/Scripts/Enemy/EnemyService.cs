using Game.Event;
using Game.Misc;
using Game.Player;
using Game.Spawn;
using Game.Wave;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyService : ISpawn<EnemySpawnData>
    {
        // Private Variables
        private EnemyConfig enemyConfig;
        private Transform enemyParentPanel;

        private EnemyPool enemyPool;

        // Private Services
        private EventService eventService;
        private MiscService miscService;
        private PlayerService playerService;

        public EnemyService(EnemyConfig _enemyConfig, Transform _parentPanel)
        {
            // Setting Variables
            enemyConfig = _enemyConfig;
            enemyParentPanel = _parentPanel;
        }

        public void Init(EventService _eventService, MiscService _miscService, PlayerService _playerService)
        {
            // Setting Services
            eventService = _eventService;
            miscService = _miscService;
            playerService = _playerService;

            // Setting Elements
            enemyPool = new EnemyPool(enemyConfig, enemyParentPanel, _eventService, _miscService, _playerService);
        }

        public void Reset()
        {
            DestroyEnemies(true);
        }

        public void Update()
        {
            UpdateEnemies();
            DestroyEnemies(false);
        }
        private void UpdateEnemies()
        {
            for (int i = enemyPool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's IsUsed is false
                if (!enemyPool.pooledItems[i].IsUsed)
                {
                    continue;
                }

                var enemyController = enemyPool.pooledItems[i].Item;
                enemyController.Update();
            }
        }
        private void DestroyEnemies(bool _isAllFlag)
        {
            for (int i = enemyPool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's IsUsed is false
                if (!enemyPool.pooledItems[i].IsUsed && !_isAllFlag)
                {
                    continue;
                }

                var enemyController = enemyPool.pooledItems[i].Item;
                if (!enemyController.IsActive() || _isAllFlag)
                {
                    ReturnEnemyToPool(enemyController);
                }
            }
        }

        public void OnSpawn(Func<Vector3> _spawnPositionFunc, EnemySpawnData _spawnData)
        {
            for (int i = 0; i < _spawnData.enemyCount; ++i)
            {
                CreateEnemy(_spawnData.enemyType, _spawnPositionFunc());
            }

            eventService.OnEnemyCountUIUpdateEvent.Invoke(EnemiesAliveCount());
        }

        private EnemyController CreateEnemy(EnemyType _enemyType, Vector3 _spawnPosition)
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

            eventService.OnEnemyCountUIUpdateEvent.Invoke(EnemiesAliveCount());
        }

        // Getters
        public int EnemiesAliveCount() => enemyPool.pooledItems.Count(item => item.IsUsed);
    }
}