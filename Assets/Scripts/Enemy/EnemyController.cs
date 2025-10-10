using Game.Event;
using Game.Player;
using UnityEngine;

namespace Game.Enemy
{
    public abstract class EnemyController
    {
        // Private Variables
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyAnimationController enemyAnimationController;

        private EnemyStateMachine enemyStateMachine;

        private int currentHealth;

        // Private Services
        public EventService EventService { get; private set; }
        private PlayerService playerService;

        public EnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            EventService _eventService, PlayerService _playerService)
        {
            // Setting Variables
            enemyModel = new EnemyModel(_enemyData);
            enemyView = Object.Instantiate(_enemyData.enemyPrefab, _parentPanel).GetComponent<EnemyView>();
            enemyView.Init(this);
            enemyAnimationController = new EnemyAnimationController(enemyView.GetAnimator(), this);

            // Setting Services
            EventService = _eventService;
            playerService = _playerService;

            // Setting Elements
            CreateStateMachine();
            Reset(_enemyData, _spawnPosition);
        }

        private void CreateStateMachine()
        {
            enemyStateMachine = new EnemyStateMachine(this);
        }

        public void Reset(EnemyData _enemyData, Vector3 _spawnPosition)
        {
            // Setting Variables
            enemyStateMachine.ChangeState(EnemyState.IDLE);
            enemyModel.Reset(_enemyData);

            currentHealth = enemyModel.MaxHealth;

            enemyView.SetPosition(_spawnPosition);
            enemyView.SetTrailRenderActive(false);
            enemyView.ShowView();
        }

        public void Update()
        {
            enemyStateMachine.Update();
        }

        public void RotateTowardsPlayer()
        {
            Transform enemyTransform = GetTransform();
            Vector3 direction = (playerService.GetController().GetTransform().position -
                enemyTransform.position).normalized;

            direction.y = 0f;
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(
                    enemyTransform.rotation,
                    targetRotation,
                    Time.deltaTime * enemyModel.RotationSpeed
                );
        }

        public void HitImpact(int _damage)
        {
            DecreaseHealth(_damage);

            if (currentHealth == 0)
            {
                enemyStateMachine.ChangeState(EnemyState.DEAD);
            }
        }
        private void DecreaseHealth(int _damage)
        {
            currentHealth -= _damage;
            if (currentHealth < 0)
                currentHealth = 0;
        }

        // Getters
        public float GetDistanceFromPlayer()
        {
            float distance = Vector3.Distance(enemyView.transform.position, GetPlayerPosition());
            return distance;
        }
        public Vector3 GetPlayerPosition() => playerService.GetController().GetTransform().position;
        public bool IsActive() => enemyView.gameObject.activeInHierarchy;
        public EnemyModel GetModel() => enemyModel;
        public EnemyView GetView() => enemyView;
        public EnemyAnimationController GetAnimationController() => enemyAnimationController;
        public EnemyStateMachine GetEnemyStateMachine() => enemyStateMachine;
        public Transform GetTransform() => enemyView.transform;
    }
}