using Game.Utility;
using UnityEngine;

namespace Game.Wave
{
    public class WaveStartState : IState<WaveService>
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

            Owner.EventService.OnWaveUIUpdateEvent.Invoke(Owner.CurrentWaveType);
            Owner.EventService.OnLoadTextUIUpdateEvent.Invoke(true);
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