using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyHurtState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyHurtState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().hurtHash);
        }
        public void Update()
        {
            if (IsHurtAnimationFinished())
            {
                stateMachine.ChangeState(EnemyState.STUN);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }

        private bool IsHurtAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().hurtHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}