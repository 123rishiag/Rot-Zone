using Game.Utility;

namespace Game.Enemy
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

    public class EnemyStateMachine : GenericStateMachine<EnemyState, EnemyController>
    {
        public EnemyStateMachine(EnemyController _owner) : base(_owner) { }

        protected override void CreateStates()
        {
            AddState(EnemyState.IDLE, new EnemyIdleState(this));
            AddState(EnemyState.PATROL, new EnemyPatrolState(this));
            AddState(EnemyState.DETECT, new EnemyDetectState(this));
            AddState(EnemyState.CHASE, new EnemyChaseState(this));
            AddState(EnemyState.ATTACK, new EnemyAttackState(this));
            AddState(EnemyState.HURT, new EnemyHurtState(this));
            AddState(EnemyState.STUN, new EnemyStunState(this));
            AddState(EnemyState.DEAD, new EnemyDeadState(this));
        }
    }
}