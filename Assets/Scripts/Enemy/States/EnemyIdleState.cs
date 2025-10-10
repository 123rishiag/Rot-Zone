using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyIdleState : IState<EnemyController>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float idleTimer;

        public EnemyIdleState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            idleTimer = 0f;

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

            if (Owner.GetDistanceFromPlayer() <= Owner.GetModel().DetectionMinDistance)
            {
                stateMachine.ChangeState(EnemyState.DETECT);
            }
            else if (idleTimer > Owner.GetModel().IdleDuration)
            {
                stateMachine.ChangeState(EnemyState.PATROL);
            }
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}