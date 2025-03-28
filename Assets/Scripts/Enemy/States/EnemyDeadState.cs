using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyDeadState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float deadTimer;
        private const float hideDuration = 3f;

        public EnemyDeadState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            deadTimer = 0f;

            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().enabled = false;
            Owner.GetView().SetRagDollActive(true);
            Owner.GetView().GetCharacterController().enabled = false;
            Owner.GetView().GetNavMeshAgent().enabled = false;

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().deadHash);
        }
        public void Update()
        {
            deadTimer += Time.deltaTime;
            if (deadTimer >= hideDuration)
            {
                Owner.GetView().HideView();
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().GetAnimator().enabled = true;
            Owner.GetView().SetRagDollActive(false);
            Owner.GetView().GetCharacterController().enabled = true;
            Owner.GetView().GetNavMeshAgent().enabled = true;
        }
    }
}