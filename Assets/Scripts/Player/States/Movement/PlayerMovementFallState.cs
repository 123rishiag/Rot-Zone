using Game.Utility;
using Game.Weapon;

namespace Game.Player
{
    public class PlayerMovementFallState<T> : IState<PlayerController, PlayerMovementState>
    {
        public PlayerController Owner { get; set; }
        private PlayerMovementStateMachine stateMachine;

        public PlayerMovementFallState(PlayerMovementStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetWeaponController().EquipWeapon(WeaponType.NONE);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().fallHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateMovementVariables();
            Owner.MovePlayer();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            if (Owner.IsGrounded())
            {
                stateMachine.ChangeState(PlayerMovementState.IDLE);
            }
        }
    }
}