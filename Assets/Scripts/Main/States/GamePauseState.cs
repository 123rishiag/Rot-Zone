using ServiceLocator.Sound;
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
            Owner.IsPausePressed = false;

            Owner.GetUIService().GetController().EnablePauseMenuPanel(true); // Show Pause Menu
            Owner.GetEventService().OnPlaySoundEffectEvent.Invoke(SoundType.GAME_PAUSE);
        }
        public void Update()
        {
            CheckGameResume();
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetController().EnablePauseMenuPanel(false); // Hide Pause Menu
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