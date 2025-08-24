using Game.Utility;

namespace Game.Wave
{
    public enum WaveState
    {
        START,
        PROGRESS,
        END,
    }

    public class WaveStateMachine : GenericStateMachine<WaveState, WaveService>
    {
        public WaveStateMachine(WaveService _owner) : base(_owner) { }

        protected override void CreateStates()
        {
            AddState(WaveState.START, new WaveStartState(this));
            AddState(WaveState.PROGRESS, new WaveProgressState(this));
            AddState(WaveState.END, new WaveEndState(this));
        }
    }
}