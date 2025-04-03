using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Wave
{
    public class WaveStartState<T> : IState<WaveService, WaveState>
    {
        public WaveService Owner { get; set; }
        private WaveStateMachine stateMachine;

        private float startTimer;
        private const float startDuration = 3f;

        public WaveStartState(WaveStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            startTimer = 0f;
            Owner.PlayerService.Reset();
            Owner.InputService.DisableControls();

            Owner.UIService.GetController().UpdateWaveText(Owner.CurrentWaveType.ToString());
            Owner.UIService.GetController().UpdateMessageText("Loading");
        }
        public void Update()
        {
            startTimer += Time.deltaTime;
            if (startTimer >= startDuration)
            {
                stateMachine.ChangeState(WaveState.PROGRESS);
            }
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}