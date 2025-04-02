using ServiceLocator.Utility;

namespace ServiceLocator.Wave
{
    public class WaveEndState<T> : IState<WaveService, WaveState>
    {
        public WaveService Owner { get; set; }
        private WaveStateMachine stateMachine;

        public WaveEndState(WaveStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter() { }
        public void Update()
        {
            if (!Owner.IsLastWave())
            {
                stateMachine.ChangeState(WaveState.START);
                Owner.SetNextWave();
            }
            else
            {
                Owner.IsLastWaveComplete = true;
            }
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}