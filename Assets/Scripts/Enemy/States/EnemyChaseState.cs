using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyChaseState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private Vector3 detectTarget;

        public EnemyChaseState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();
            agent.isStopped = false;

            agent.speed = enemyModel.ChaseSpeed;
            agent.acceleration = enemyModel.ChaseSpeed * 2;

            agent.stoppingDistance = enemyModel.StopDistance;

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().chaseHash, 0.1f);
        }
        public void Update()
        {
            detectTarget = Owner.PlayerService.GetController().GetTransform().position;

            float distance = Vector3.Distance(Owner.GetTransform().position, detectTarget);
            if (distance > Owner.GetModel().DetectionMaxDistance)         
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }
            else if (distance <= Owner.GetModel().StopDistance)
            {
                stateMachine.ChangeState(EnemyState.ATTACK);
            }

            Owner.GetView().GetNavMeshAgent().destination = detectTarget;
            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}