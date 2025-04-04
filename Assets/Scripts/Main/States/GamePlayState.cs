using ServiceLocator.Sound;
using ServiceLocator.Utility;
using ServiceLocator.Wave;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GamePlayState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GamePlayState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Time.timeScale = 1f; // Resume the game
            Owner.IsPausePressed = false;
            Owner.GetEventService().OnPlaySoundEffectEvent.Invoke(SoundType.GAME_PLAY);
        }
        public void Update()
        {
            CheckGameEndCondition();
            if (Owner.GetWaveService().GetWaveStateMachine().GetCurrentState() == WaveState.PROGRESS)
            {
                CheckPlayerDeadCondition();
                CheckGamePause();

                // Event Service
                // Input Service
                // Camera Service
                // Sound Service
                // UI Service
                Owner.GetProjectileService().Update();
                // Weapon Service
                Owner.GetPlayerService().Update();
                Owner.GetEnemyService().Update();
                // Spawn Service
            }
            Owner.GetWaveService().Update();
        }
        public void FixedUpdate()
        {
            // Event Service
            // Input Service
            // Camera Service
            // Sound Service
            // UI Service
            // Projectile Service
            // Weapon Service
            // Player Service
            // Enemy Service
            // Spawn Service
            // Wave Service
        }
        public void LateUpdate()
        {
            if (Owner.GetWaveService().GetWaveStateMachine().GetCurrentState() == WaveState.PROGRESS)
            {
                // Event Service
                // Input Service
                Owner.GetCameraService().LateUpdate();
                // Sound Service
                // UI Service
                // Projectile Service
                Owner.GetWeaponService().LateUpdate();
                // Player Service
                // Enemy Service
                // Spawn Service
            }
            // Wave Service
        }
        public void OnStateExit()
        {
            Time.timeScale = 0f; // Stop the game
        }

        private void CheckGamePause()
        {
            if (Owner.IsPausePressed)
            {
                stateMachine.ChangeState(GameState.GAME_PAUSE);
            }
        }
        private void CheckGameEndCondition()
        {
            if (Owner.GetWaveService().IsLastWaveComplete)
            {
                stateMachine.ChangeState(GameState.GAME_OVER);
            }
        }
        private void CheckPlayerDeadCondition()
        {
            if (!Owner.GetPlayerService().IsPlayerAlive())
            {
                stateMachine.ChangeState(GameState.GAME_OVER);
            }
        }
    }
}