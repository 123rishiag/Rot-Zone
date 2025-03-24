using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyIdleState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float idleTimer;

        public EnemyIdleState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            idleTimer = 0f;

            var agent = Owner.GetView().GetNavMeshAgent();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().idleHash, 0.25f);
        }
        public void Update()
        {
            idleTimer += Time.deltaTime;

            if (Owner.IsPlayerDetected())
            {
                stateMachine.ChangeState(EnemyState.DETECT);
                return;
            }
            else if (idleTimer > Owner.GetModel().IdleDuration)
            {
                stateMachine.ChangeState(EnemyState.PATROL);
            }
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}