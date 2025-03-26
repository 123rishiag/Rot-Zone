using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyAttackState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyAttackState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().SetTrailRenderActive(true);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().attackHash);
        }
        public void Update()
        {
            if (IsAttackAnimationFinished())
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }

            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().SetTrailRenderActive(false);
        }

        private bool IsAttackAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().attackHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}