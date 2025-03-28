using ServiceLocator.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace ServiceLocator.Enemy
{
    public class EnemyPatrolState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private Vector3 patrolTarget;

        public EnemyPatrolState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var agent = Owner.GetView().GetNavMeshAgent();
            var enemyModel = Owner.GetModel();
            agent.isStopped = false;

            agent.speed = enemyModel.PatrolSpeed;
            agent.acceleration = enemyModel.PatrolSpeed;

            SetNewPatrolTarget();

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().patrolHash, 0.1f);
        }
        public void Update()
        {
            if (Owner.IsPlayerDetected() || Owner.GetDistanceFromPlayer() <= Owner.GetModel().DetectionMinDistance)
            {
                stateMachine.ChangeState(EnemyState.DETECT);
            }
            else if (Vector3.Distance(Owner.GetTransform().position, patrolTarget) < Owner.GetModel().StopDistance)
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }

            Owner.GetView().GetNavMeshAgent().destination = patrolTarget;
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }

        private void SetNewPatrolTarget()
        {
            float patrolMaxRadius = Owner.GetModel().PatrolMaxRadius;
            Vector3 randomDirection = Random.insideUnitSphere * patrolMaxRadius;
            randomDirection += Owner.GetTransform().position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolMaxRadius, NavMesh.AllAreas))
            {
                patrolTarget = hit.position;
            }
        }
    }
}