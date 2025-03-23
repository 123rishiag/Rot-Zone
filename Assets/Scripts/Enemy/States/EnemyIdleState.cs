using ServiceLocator.Utility;

namespace ServiceLocator.Enemy
{
    public class EnemyIdleState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyIdleState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().idleHash);
        }
        public void Update()
        {
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}