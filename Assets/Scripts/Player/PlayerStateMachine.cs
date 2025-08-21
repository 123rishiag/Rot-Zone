using Game.Utility;

namespace Game.Player
{
    public enum PlayerMovementState
    {
        IDLE,
        WALK,
        RUN,
        TURN_IN_PLACE,
        FALL,
        HURT,
        DEAD,
    }

    public enum PlayerActionState
    {
        NONE,
        AIM,
        FIRE,
        RELOAD,
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
            States.Add(PlayerMovementState.IDLE, new PlayerMovementIdleState<PlayerController>(this));
            States.Add(PlayerMovementState.WALK, new PlayerMovementWalkState<PlayerController>(this));
            States.Add(PlayerMovementState.RUN, new PlayerMovementRunState<PlayerController>(this));
            States.Add(PlayerMovementState.TURN_IN_PLACE, new PlayerMovementTurnInPlaceState<PlayerController>(this));
            States.Add(PlayerMovementState.FALL, new PlayerMovementFallState<PlayerController>(this));
            States.Add(PlayerMovementState.HURT, new PlayerMovementHurtState<PlayerController>(this));
            States.Add(PlayerMovementState.DEAD, new PlayerMovementDeadState<PlayerController>(this));
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
            States.Add(PlayerActionState.RELOAD, new PlayerActionReloadState<PlayerController>(this));
        }
    }
}