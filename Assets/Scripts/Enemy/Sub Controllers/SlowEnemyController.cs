using Game.Event;
using Game.Player;
using UnityEngine;

namespace Game.Enemy
{
    public class SlowEnemyController : EnemyController
    {
        public SlowEnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            EventService _eventService, PlayerService _playerService) : base(_enemyData, _parentPanel, _spawnPosition,
            _eventService, _playerService)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}