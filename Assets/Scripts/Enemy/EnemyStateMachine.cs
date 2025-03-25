using ServiceLocator.Utility;

namespace ServiceLocator.Enemy
{
    public enum EnemyState
    {
        IDLE,
        PATROL,
        DETECT,
        CHASE,
    }

    public class EnemyStateMachine : GenericStateMachine<EnemyController, EnemyState>
    {
        public EnemyStateMachine(EnemyController _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(EnemyState.IDLE, new EnemyIdleState<EnemyController>(this));
            States.Add(EnemyState.PATROL, new EnemyPatrolState<EnemyController>(this));
            States.Add(EnemyState.DETECT, new EnemyDetectState<EnemyController>(this));
            States.Add(EnemyState.CHASE, new EnemyChaseState<EnemyController>(this));
        }
    }
}