using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyStunState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float stunTimer;
        private const float recoveryDuration = 2f;

        public EnemyStunState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            stunTimer = 0f;

            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().stunHash, 0.5f);
        }
        public void Update()
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= recoveryDuration)
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}