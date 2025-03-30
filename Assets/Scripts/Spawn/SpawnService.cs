using ServiceLocator.Enemy;
using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Spawn
{
    public class SpawnService
    {
        // Private Variables
        private Transform playerTransformPanel;
        private Transform enemyTransformPanel;

        // Private Services
        private PlayerService playerService;
        private EnemyService enemyService;

        public SpawnService(Transform _parentPanel)
        {
            // Setting Variables
            playerTransformPanel = _parentPanel.Find("PlayerSpawnTransforms")?.transform;
            enemyTransformPanel = _parentPanel.Find("EnemySpawnTransforms")?.transform;

            if (playerTransformPanel == null || enemyTransformPanel == null)
            {
                Debug.LogError("Spawn Transform panels not found under parent panel.");
            }
        }

        public void Init(PlayerService _playerService, EnemyService _enemyService)
        {
            // Setting Services
            playerService = _playerService;
            enemyService = _enemyService;

            Spawn(SpawnEntityType.PLAYER, 1);
            Spawn(SpawnEntityType.ENEMY, 10);
        }

        public void Spawn(SpawnEntityType _spawnEntityType, int _spawnCount)
        {
            ISpawn spawnEntity = GetSpawnEntity(_spawnEntityType);
            for (int i = 0; i < _spawnCount; ++i)
            {
                spawnEntity.OnSpawn(GetSpawnPosition(_spawnEntityType));
            }
        }

        public ISpawn GetSpawnEntity(SpawnEntityType _spawnEntityType)
        {
            switch (_spawnEntityType)
            {
                case SpawnEntityType.PLAYER:
                    return playerService;
                case SpawnEntityType.ENEMY:
                    return enemyService;
                default:
                    Debug.LogError($"Unhandled Spawn Entity Type: {_spawnEntityType}");
                    return null;
            }
        }

        private Vector3 GetSpawnPosition(SpawnEntityType _spawnEntityType)
        {
            switch (_spawnEntityType)
            {
                case SpawnEntityType.PLAYER:
                    return GetRandomChild(playerTransformPanel).position;
                case SpawnEntityType.ENEMY:
                    return GetRandomChild(enemyTransformPanel).position;
                default:
                    Debug.LogError($"No Spawn Transforms Found for Spawn Entity : {_spawnEntityType}");
                    return Vector3.zero;
            }
        }

        private Transform GetRandomChild(Transform _parentPanel)
        {
            if (_parentPanel.childCount == 0)
            {
                Debug.LogError("No children found under spawn transform panel.");
                return _parentPanel;
            }

            int randomIndex = Random.Range(0, _parentPanel.childCount);
            return _parentPanel.GetChild(randomIndex);
        }
    }
}
