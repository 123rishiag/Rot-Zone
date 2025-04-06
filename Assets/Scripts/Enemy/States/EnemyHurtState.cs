using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyHurtState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        public EnemyHurtState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            var enemyModel = Owner.GetModel();
            Owner.GetView().StopNavMeshAgent(true);

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().hurtHash);
            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.ENEMY_HURT);
        }
        public void Update()
        {
            if (IsHurtAnimationFinished())
            {
                stateMachine.ChangeState(EnemyState.STUN);
            }
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private bool IsHurtAnimationFinished()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().hurtHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}