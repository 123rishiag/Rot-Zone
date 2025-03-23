using ServiceLocator.Player;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyController : IStateOwner<EnemyController>
    {
        // Private Variables
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyAnimationController enemyAnimationController;

        public EnemyController Owner { get; set; }
        private EnemyStateMachine enemyStateMachine;

        public EnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            PlayerService _playerService)
        {
            // Setting Variables
            enemyModel = new EnemyModel(_enemyData);
            enemyView = Object.Instantiate(_enemyData.enemyPrefab, _parentPanel).GetComponent<EnemyView>();
            enemyView.Init();
            enemyAnimationController = new EnemyAnimationController(enemyView.GetAnimator(), this);

            // Setting Elements
            CreateStateMachine();
            Reset(_enemyData, _spawnPosition);
        }

        private void CreateStateMachine()
        {
            Owner = this;
            enemyStateMachine = new EnemyStateMachine(this);
        }

        public void Reset(EnemyData _enemyData, Vector3 _spawnPosition)
        {
            // Setting Variables
            enemyStateMachine.ChangeState(EnemyState.IDLE);

            enemyModel.Reset(_enemyData);
            enemyView.SetPosition(_spawnPosition);
            enemyView.ShowView();
        }

        public bool IsActive() => enemyView.gameObject.activeInHierarchy;

        // Getters
        public EnemyModel GetModel() => enemyModel;
        public EnemyView GetView() => enemyView;
        public EnemyAnimationController GetAnimationController() => enemyAnimationController;
        public EnemyStateMachine GetEnemyStateMachine() => enemyStateMachine;
    }
}