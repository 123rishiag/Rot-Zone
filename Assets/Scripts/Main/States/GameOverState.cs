using ServiceLocator.Sound;
using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GameOverState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameOverState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetController().EnableGameOverMenuPanel(true); // Show Game Over Menu
            Owner.GetEventService().OnPlaySoundEffectEvent.Invoke(SoundType.GAME_OVER);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetController().EnableGameOverMenuPanel(false); // Hide Game Over Menu
        }
    }
}