using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerMovementHurtState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementHurtState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().hurtHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            //Owner.UpdateMovementVariables();
            //Owner.MovePlayer();
        }

        public void FixedUpdate() { }
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