using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyDetectState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyDetectState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().detectHash, 0.1f);
        }
        public void Update()
        {
            if (IsDetectAnimationFinished())
            {
                stateMachine.ChangeState(EnemyState.CHASE);
            }

            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }

        private bool IsDetectAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().detectHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}