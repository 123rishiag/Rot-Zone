using ServiceLocator.Utility;

namespace ServiceLocator.Enemy
{
    public enum EnemyState
    {
        IDLE,
        PATROL,
        DETECT,
        CHASE,
        ATTACK,
        HURT,
        STUN,
        DEAD,
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
            States.Add(EnemyState.ATTACK, new EnemyAttackState<EnemyController>(this));
            States.Add(EnemyState.HURT, new EnemyHurtState<EnemyController>(this));
            States.Add(EnemyState.STUN, new EnemyStunState<EnemyController>(this));
            States.Add(EnemyState.DEAD, new EnemyDeadState<EnemyController>(this));
        }
    }
}