using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttackState : IState<EnemyController>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyAttackState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().StopNavMeshAgent(true);

            Owner.GetView().SetTrailRenderActive(true);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().attackHash);
            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.ENEMY_ATTACK);
        }
        public void Update()
        {
            if (IsAttackAnimationFinished())
            {
                if (Owner.GetDistanceFromPlayer() > Owner.GetModel().DetectionMaxDistance)
                {
                    stateMachine.ChangeState(EnemyState.IDLE);
                }
                else
                {
                    stateMachine.ChangeState(EnemyState.CHASE);
                }
            }

            Owner.RotateTowardsPlayer();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().SetTrailRenderActive(false);
        }

        private bool IsAttackAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().attackHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}