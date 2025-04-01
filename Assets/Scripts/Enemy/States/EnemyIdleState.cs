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

            Owner.GetView().GetAnimator().enabled = true;
            Owner.GetView().SetRagDollActive(false);
            Owner.GetView().GetCharacterController().enabled = true;
            Owner.GetView().GetNavMeshAgent().enabled = true;

            Owner.GetView().StopNavMeshAgent(true);
            var animator = Owner.GetView().GetAnimator();
            if (animator != null && animator.isActiveAndEnabled)
            {
                animator.CrossFade(Owner.GetAnimationController().idleHash, 0.25f);
            }
            
        }
        public void Update()
        {
            idleTimer += Time.deltaTime;

            if (Owner.IsPlayerDetected() || Owner.GetDistanceFromPlayer() <= Owner.GetModel().DetectionMinDistance)
            {
                stateMachine.ChangeState(EnemyState.DETECT);
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