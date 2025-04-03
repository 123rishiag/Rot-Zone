using ServiceLocator.Utility;

namespace ServiceLocator.Wave
{
    public class WaveProgressState<T> : IState<WaveService, WaveState>
    {
        public WaveService Owner { get; set; }
        private WaveStateMachine stateMachine;

        public WaveProgressState(WaveStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.InputService.EnableControls();
            Owner.LoadCurrentWave();

            Owner.EventService.OnMessageUIUpdateEvent.Invoke("");
        }
        public void Update()
        {
            if (Owner.IsWaveComplete())
            {
                stateMachine.ChangeState(WaveState.END);
            }
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}