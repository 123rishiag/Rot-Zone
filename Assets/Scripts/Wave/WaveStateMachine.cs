using Game.Utility;

namespace Game.Wave
{
    public enum WaveState
    {
        START,
        PROGRESS,
        END,
    }

    public class WaveStateMachine : GenericStateMachine<WaveService, WaveState>
    {
        public WaveStateMachine(WaveService _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(WaveState.START, new WaveStartState<WaveService>(this));
            States.Add(WaveState.PROGRESS, new WaveProgressState<WaveService>(this));
            States.Add(WaveState.END, new WaveEndState<WaveService>(this));
        }
    }
}