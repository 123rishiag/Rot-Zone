using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GamePauseState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GamePauseState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetUIController().EnablePauseMenuPanel(true); // Show Pause Menu
        }
        public void Update()
        {
            CheckGameResume();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetUIController().EnablePauseMenuPanel(false); // Hide Pause Menu
        }

        private void CheckGameResume()
        {
            if (Owner.IsPausePressed)
            {
                stateMachine.ChangeState(GameState.GAME_PLAY);
            }
        }
    }
}