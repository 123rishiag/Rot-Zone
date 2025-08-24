using Game.Utility;

namespace Game.Main
{
    public class GameRestartState : IState<GameController>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameRestartState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.Reset();
            stateMachine.ChangeState(GameState.GAME_PLAY);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}