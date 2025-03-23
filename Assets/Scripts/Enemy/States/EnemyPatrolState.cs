using ServiceLocator.Utility;

namespace ServiceLocator.Enemy
{
    public class EnemyPatrolState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyPatrolState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
        }
        public void Update()
        {
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}