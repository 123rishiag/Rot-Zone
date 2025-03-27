using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyHurtState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float hurtTimer;
        private const float recoveryDuration = 0.1f;

        public EnemyHurtState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            hurtTimer = 0f;

            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Owner.GetView().GetAnimator().enabled = false;
            Owner.GetView().SetRagDollActive(true);

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().hurtHash);
        }
        public void Update()
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer >= recoveryDuration)
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().GetAnimator().enabled = true;
            Owner.GetView().SetRagDollActive(false);
        }
    }
}