using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyDeadState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyDeadState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().enabled = false;
            Owner.GetView().SetRagDollActive(true);

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().deadHash);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().GetAnimator().enabled = true;
            Owner.GetView().SetRagDollActive(false);
        }
    }
}