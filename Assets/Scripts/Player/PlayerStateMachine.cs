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

    public class PlayerMovementStateMachine : GenericStateMachine<PlayerMovementState, PlayerController>
    {
        public PlayerMovementStateMachine(PlayerController _owner) : base(_owner) { }

        protected override void CreateStates()
        {
            AddState(PlayerMovementState.IDLE, new PlayerMovementIdleState(this));
            AddState(PlayerMovementState.WALK, new PlayerMovementWalkState(this));
            AddState(PlayerMovementState.RUN, new PlayerMovementRunState(this));
            AddState(PlayerMovementState.TURN_IN_PLACE, new PlayerMovementTurnInPlaceState(this));
            AddState(PlayerMovementState.FALL, new PlayerMovementFallState(this));
            AddState(PlayerMovementState.HURT, new PlayerMovementHurtState(this));
            AddState(PlayerMovementState.DEAD, new PlayerMovementDeadState(this));
        }
    }

    public class PlayerActionStateMachine : GenericStateMachine<PlayerActionState, PlayerController>
    {
        public PlayerActionStateMachine(PlayerController _owner) : base(_owner) { }

        protected override void CreateStates()
        {
            AddState(PlayerActionState.NONE, new PlayerActionNoneState(this));
            AddState(PlayerActionState.AIM, new PlayerActionAimState(this));
            AddState(PlayerActionState.FIRE, new PlayerActionFireState(this));
            AddState(PlayerActionState.RELOAD, new PlayerActionReloadState(this));
        }
    }
}