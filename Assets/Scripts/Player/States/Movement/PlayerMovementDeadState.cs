using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerMovementDeadState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementDeadState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().SetRagDollActive(true);
            Owner.GetView().GetAnimator().enabled = false;
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().fallHash);
        }
        public void Update()
        {
            Owner.UpdateMovementVariables();
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetView().SetRagDollActive(false);
            Owner.GetView().GetAnimator().enabled = true;
        }
    }
}