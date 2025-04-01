using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Wave
{
    public class WaveEndState<T> : IState<WaveService, WaveState>
    {
        public WaveService Owner { get; set; }
        private WaveStateMachine stateMachine;

        private float endTimer;
        private const float endDuration = 3f;

        public WaveEndState(WaveStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            endTimer = 0f;
        }
        public void Update()
        {
            endTimer += Time.deltaTime;
            if (endTimer >= endDuration)
            {
                if (!Owner.IsLastWave())
                {
                    stateMachine.ChangeState(WaveState.START);
                }
                else
                {
                    Owner.IsLastWaveComplete = true;
                }
            }
        }

        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.SetNextWave();
        }
    }
}