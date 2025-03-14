using ServiceLocator.Utility;
using ServiceLocator.Weapon;

namespace ServiceLocator.Player
{
    public class PlayerActionAimState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionAimState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponIdleHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateActionVariables();
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            if (Owner.GetWeaponVisualController().GetCurrentWeapon() != WeaponType.NONE && Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.FIRE);
            }
            else if (Owner.GetWeaponVisualController().GetCurrentWeapon() == WeaponType.NONE)
            {
                stateMachine.ChangeState(PlayerActionState.NONE);
            }
        }
    }
}