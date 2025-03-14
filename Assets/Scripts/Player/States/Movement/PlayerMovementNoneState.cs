using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerMovementNoneState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementNoneState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter() { }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}