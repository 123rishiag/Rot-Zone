using Game.Utility;

namespace Game.Player
{
    public class PlayerMovementRunState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementRunState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetAnimationController().SelectLocomotionHash();
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateMovementVariables();
            Owner.MovePlayer();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            if (!Owner.IsGrounded())
            {
                stateMachine.ChangeState(PlayerMovementState.FALL);
            }
            else if (Owner.GetCurrentSpeed() == 0)
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
            else if (Owner.GetCurrentSpeed() > 0f && Owner.GetCurrentSpeed() <= Owner.GetModel().WalkSpeed)
            {
                stateMachine.ChangeState(PlayerMovementState.WALK);
            }
        }
    }
}