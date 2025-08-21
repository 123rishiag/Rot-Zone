using Game.Utility;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementTurnInPlaceState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        private float fullTurnAngle = 180f;

        public PlayerMovementTurnInPlaceState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().turnLocomotionHash, 0.1f);
        }
        public void Update()
        {
            CheckTransitionConditions();
            ApplyTurnAnimation();

            Owner.UpdateMovementVariables();
            Owner.MovePlayer();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (!Owner.IsGrounded())
            {
                stateMachine.ChangeState(PlayerMovementState.FALL);
            }
            else if (Owner.GetCurrentSpeed() > 0f && Owner.GetCurrentSpeed() <= Owner.GetModel().WalkSpeed)
            {
                stateMachine.ChangeState(PlayerMovementState.WALK);
            }
            else if (Owner.GetCurrentSpeed() > Owner.GetModel().WalkSpeed &&
                Owner.GetCurrentSpeed() <= Owner.GetModel().RunSpeed)
            {
                stateMachine.ChangeState(PlayerMovementState.RUN);
            }
            else if (stateInfo.shortNameHash == Owner.GetAnimationController().turnLocomotionHash &&
                stateInfo.normalizedTime >= 0.5f)
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
        }
        private void ApplyTurnAnimation()
        {
            float currentYaw = Owner.GetView().transform.eulerAngles.y;
            float targetYaw = Owner.GetRotationTarget().y;
            float deltaYaw = Mathf.DeltaAngle(currentYaw, targetYaw);
            float turn = Mathf.Clamp(deltaYaw / fullTurnAngle, -1f, 1f) * -1f;
            Owner.GetView().GetAnimator().SetFloat(Owner.GetAnimationController().turnHash, turn);
        }
    }
}