using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class SlowEnemyController : EnemyController
    {
        public SlowEnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            PlayerService _playerService) : base(_enemyData, _parentPanel, _spawnPosition,
            _playerService)
        {
        }
    }
}