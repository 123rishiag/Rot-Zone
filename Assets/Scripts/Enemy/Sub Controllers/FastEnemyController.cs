using Game.Event;
using Game.Misc;
using Game.Player;
using UnityEngine;

namespace Game.Enemy
{
    public class FastEnemyController : EnemyController
    {
        public FastEnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            EventService _eventService, MiscService _miscService, PlayerService _playerService) : base(_enemyData, _parentPanel, _spawnPosition,
             _eventService, _miscService, _playerService)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}