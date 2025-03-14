using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerActionAimState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionAimState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter() { }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}