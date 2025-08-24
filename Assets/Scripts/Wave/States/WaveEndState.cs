using Game.Utility;

namespace Game.Wave
{
    public class WaveEndState : IState<WaveService>
    {
        public WaveService Owner { get; set; }
        private WaveStateMachine stateMachine;

        public WaveEndState(WaveStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.InputService.DisableControls();
        }
        public void Update()
        {
            if (!Owner.IsLastWave())
            {
                Owner.SetNextWave();
                stateMachine.ChangeState(WaveState.START);
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