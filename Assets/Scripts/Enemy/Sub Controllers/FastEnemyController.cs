using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class FastEnemyController : EnemyController
    {
        public FastEnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            PlayerService _playerService) : base(_enemyData, _parentPanel, _spawnPosition,
            _playerService)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}