using ServiceLocator.Utility;

namespace ServiceLocator.Enemy
{
    public class EnemyChaseState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyChaseState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.DetectionDistance = Owner.GetModel().DetectionMaxDistance * Owner.GetModel().DetectionIncreaseFactor;
            Owner.GetView().SetConeDetectMaterial(true);

            var enemyModel = Owner.GetModel();
            var agent = Owner.GetView().GetNavMeshAgent();
            Owner.GetView().StopNavMeshAgent(false);

            agent.speed = enemyModel.ChaseSpeed;
            agent.acceleration = enemyModel.ChaseSpeed * 2;

            agent.stoppingDistance = enemyModel.StopDistance;

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().chaseHash, 0.1f);
        }
        public void Update()
        {
            if (Owner.GetDistanceFromPlayer() > Owner.DetectionDistance)
            {
                stateMachine.ChangeState(EnemyState.IDLE);
            }
            else if (Owner.GetDistanceFromPlayer() <= Owner.GetModel().StopDistance)
            {
                stateMachine.ChangeState(EnemyState.ATTACK);
            }

            Owner.GetView().GetNavMeshAgent().destination = Owner.GetPlayerPosition();
            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.DetectionDistance = Owner.GetModel().DetectionMaxDistance;
        }
    }
}