using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementDeadState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        private float deadTimer;
        private const float deadDuration = 3f;

        public PlayerMovementDeadState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            deadTimer = 0f;
            Owner.InputService.DisableControls();

            Owner.GetView().SetRagDollActive(true);
            Owner.GetView().GetAnimator().enabled = false;

            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.PLAYER_DEAD);
        }
        public void Update()
        {
            deadTimer += Time.deltaTime;
            if (deadTimer > deadDuration)
            {
                Owner.IsAlive = false;
            }
            Owner.UpdateMovementVariables();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().SetRagDollActive(false);
            Owner.GetView().GetAnimator().enabled = true;
        }
    }
}