using Game.Utility;

namespace Game.Main
{
    public class GameStartState : IState<GameController>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameStartState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.Reset();
            stateMachine.ChangeState(GameState.GAME_MENU);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}