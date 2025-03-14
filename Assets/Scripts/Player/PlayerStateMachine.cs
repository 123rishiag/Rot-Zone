using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public enum PlayerMovementState
    {
        NONE,
        IDLE,
        WALK,
        RUN,
        FALL,
    }

    public enum PlayerActionState
    {
        NONE,
        AIM,
        FIRE,
    }

    public class PlayerMovementStateMachine : GenericStateMachine<PlayerController, PlayerMovementState>
    {
        public PlayerMovementStateMachine(PlayerController _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(PlayerMovementState.NONE, new PlayerMovementNoneState<PlayerController>(this));
            States.Add(PlayerMovementState.IDLE, new PlayerMovementIdleState<PlayerController>(this));
            States.Add(PlayerMovementState.WALK, new PlayerMovementWalkState<PlayerController>(this));
            States.Add(PlayerMovementState.RUN, new PlayerMovementRunState<PlayerController>(this));
            States.Add(PlayerMovementState.FALL, new PlayerMovementFallState<PlayerController>(this));
        }
    }

    public class PlayerActionStateMachine : GenericStateMachine<PlayerController, PlayerActionState>
    {
        public PlayerActionStateMachine(PlayerController _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(PlayerActionState.NONE, new PlayerActionNoneState<PlayerController>(this));
            States.Add(PlayerActionState.AIM, new PlayerActionAimState<PlayerController>(this));
            States.Add(PlayerActionState.FIRE, new PlayerActionFireState<PlayerController>(this));
        }
    }
}