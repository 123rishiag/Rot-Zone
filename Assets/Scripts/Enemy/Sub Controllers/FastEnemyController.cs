using ServiceLocator.Event;
using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class FastEnemyController : EnemyController
    {
        public FastEnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            EventService _eventService, PlayerService _playerService) : base(_enemyData, _parentPanel, _spawnPosition,
             _eventService, _playerService)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}