using Game.Utility;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementTurnInPlaceState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        private float fullTurnAngle;
        private float turnVel;

        public PlayerMovementTurnInPlaceState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            fullTurnAngle = 180f;
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().turnLocomotionHash);
        }
        public void Update()
        {
            ApplyTurnAnimation();
            CheckTransitionConditions();

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
                stateInfo.normalizedTime >= 0.99f)
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
        }
        private void ApplyTurnAnimation()
        {
            // Player forward (world)
            Vector3 playerForward = Owner.GetTransform().forward;
            // Target forward (based on target yaw)
            Vector3 targetForward = Quaternion.Euler(0f, Owner.GetRotationTarget().y, 0f) * Vector3.forward;

            // Signed angle between player forward and target forward, around the up axis
            float deltaYaw = Vector3.SignedAngle(playerForward, targetForward, Vector3.up);

            // Clamp to your allowed angle range
            float targetTurn = Mathf.Clamp(deltaYaw, -fullTurnAngle, fullTurnAngle);
            float currentTurn = Owner.GetView().GetAnimator().GetFloat(Owner.GetAnimationController().turnHash);
            float smoothTurn = Mathf.SmoothDamp(currentTurn, targetTurn, ref turnVel, 0.1f);

            // Feed directly to animator
            Owner.GetView().GetAnimator().SetFloat(Owner.GetAnimationController().turnHash, smoothTurn);
        }
    }
}