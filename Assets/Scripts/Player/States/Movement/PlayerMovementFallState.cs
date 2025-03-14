using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerMovementFallState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementFallState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().fallHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateMovementVariables();
            Owner.MovePlayer();
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            if (Owner.IsGrounded())
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
        }
    }
}