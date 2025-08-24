using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementHurtState : IState<PlayerController>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementHurtState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().hurtHash);
            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.PLAYER_HURT);
        }
        public void Update()
        {
            CheckTransitionConditions();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == Owner.GetAnimationController().hurtHash &&
                stateInfo.normalizedTime >= 0.9f)
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
        }
    }
}