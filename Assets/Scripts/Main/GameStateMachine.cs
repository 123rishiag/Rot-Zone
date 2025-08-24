using Game.Utility;

namespace Game.Main
{
    public enum GameState
    {
        GAME_START,
        GAME_MENU,
        GAME_CONTROL,
        GAME_PLAY,
        GAME_PAUSE,
        GAME_RESTART,
        GAME_OVER
    }

    public class GameStateMachine : GenericStateMachine<GameState, GameController>
    {
        public GameStateMachine(GameController _owner) : base(_owner) { }

        protected override void CreateStates()
        {
            AddState(GameState.GAME_START, new GameStartState(this));
            AddState(GameState.GAME_MENU, new GameMenuState(this));
            AddState(GameState.GAME_CONTROL, new GameControlState(this));
            AddState(GameState.GAME_PLAY, new GamePlayState(this));
            AddState(GameState.GAME_PAUSE, new GamePauseState(this));
            AddState(GameState.GAME_RESTART, new GameRestartState(this));
            AddState(GameState.GAME_OVER, new GameOverState(this));
        }
    }
}
