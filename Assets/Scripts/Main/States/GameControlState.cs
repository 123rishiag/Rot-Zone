using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GameControlState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameControlState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetController().EnableControlMenuPanel(true); // Show Control Menu
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetController().EnableControlMenuPanel(false); // Hide Control Menu
        }
    }
}