using ServiceLocator.Player;
using ServiceLocator.Utility;
using System;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        // Private Variables
        private EnemyConfig enemyConfig;
        private Transform enemyParentPanel;

        private EnemyType enemyType;
        private Vector3 spawnPosition;

        // Private Services
        private PlayerService playerService;

        public EnemyPool(EnemyConfig _enemyConfig, Transform _parentPanel,
            PlayerService _playerService)
        {
            // Setting Variables
            enemyConfig = _enemyConfig;
            enemyParentPanel = _parentPanel;

            // Setting Services
            playerService = _playerService;
        }

        public EnemyController GetEnemy<T>(EnemyType _enemyType, Vector3 _spawnPosition)
            where T : EnemyController
        {
            // Setting Variables
            enemyType = _enemyType;
            spawnPosition = _spawnPosition;

            // Fetching Item
            var item = GetItem<T>();

            // Resetting Item Properties
            item.Reset(GetEnemyData(enemyType), _spawnPosition);

            return item;
        }

        protected override EnemyController CreateItem<T>()
        {
            // Creating Controller
            switch (enemyType)
            {
                case EnemyType.SLOW_ZOMBIE:
                    return new SlowEnemyController(
                        GetEnemyData(enemyType), enemyParentPanel, spawnPosition,
                        playerService);
                case EnemyType.FAST_ZOMBIE:
                    return new FastEnemyController(
                        GetEnemyData(enemyType), enemyParentPanel, spawnPosition,
                        playerService);
                default:
                    Debug.LogError($"Unhandled EnemyType: {enemyType}");
                    return null;
            }
        }

        private EnemyData GetEnemyData(EnemyType _enemyType) =>
            Array.Find(enemyConfig.enemyData, w => w.enemyType == _enemyType);
    }
}