using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyDetectState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyDetectState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.DetectionDistance = Owner.GetModel().DetectionMaxDistance * Owner.GetModel().DetectionIncreaseFactor;
            Owner.GetView().SetConeDetectMaterial(true);

            var enemyModel = Owner.GetModel();
            Owner.GetView().StopNavMeshAgent(true);

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().detectHash, 0.1f);
        }
        public void Update()
        {
            if (Owner.GetDistanceFromPlayer() <= Owner.GetModel().StopDistance)
            {
                stateMachine.ChangeState(EnemyState.ATTACK);
            }
            else if (IsDetectAnimationFinished() || Owner.GetDistanceFromPlayer() <= Owner.GetModel().DetectionMinScreamDistance)
            {
                stateMachine.ChangeState(EnemyState.CHASE);
            }

            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.DetectionDistance = Owner.GetModel().DetectionMaxDistance;
        }

        private bool IsDetectAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().detectHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}