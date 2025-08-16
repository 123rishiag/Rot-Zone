using Game.Sound;
using Game.Utility;
using Game.Wave;
using UnityEngine;

namespace Game.Main
{
    public class GamePlayState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GamePlayState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Time.timeScale = 1f; // Resume the game
            Cursor.visible = false;
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
                // Sound Service
                // UI Service
                Owner.GetProjectileService().Update();
                // Weapon Service
                Owner.GetPlayerService().Update();
                Owner.GetEnemyService().Update();
                // Spawn Service
                Owner.GetCameraService().Update();
            }
            Owner.GetWaveService().Update();
        }
        public void FixedUpdate()
        {
            if (Owner.GetWaveService().GetWaveStateMachine().GetCurrentState() == WaveState.PROGRESS)
            {
                // Event Service
                // Input Service
                // Sound Service
                // UI Service
                // Projectile Service
                // Weapon Service
                // Player Service
                // Enemy Service
                // Spawn Service
                // Camera Service
                // Wave Service
            }
        }
        public void LateUpdate()
        {
            if (Owner.GetWaveService().GetWaveStateMachine().GetCurrentState() == WaveState.PROGRESS)
            {
                // Event Service
                // Input Service
                // Sound Service
                // UI Service
                // Projectile Service
                Owner.GetWeaponService().LateUpdate();
                // Player Service
                // Enemy Service
                // Spawn Service
                // Camera Service
            }
            // Wave Service
        }
        public void OnStateExit()
        {
            Time.timeScale = 0f; // Stop the game
            Cursor.visible = true;
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