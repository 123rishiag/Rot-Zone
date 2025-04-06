using Game.Utility;

namespace Game.Main
{
    public class GameMenuState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameMenuState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetController().EnableMainMenuPanel(true); // Show Main Menu
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetController().EnableMainMenuPanel(false); // Hide Main Menu
        }
    }
}