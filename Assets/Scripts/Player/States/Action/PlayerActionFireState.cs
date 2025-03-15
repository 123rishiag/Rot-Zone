using ServiceLocator.Utility;
using ServiceLocator.Weapon;

namespace ServiceLocator.Player
{
    public class PlayerActionFireState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionFireState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetPlayerWeaponController().GetCurrentWeapon(), true);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponFireHash);
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
            if (Owner.GetPlayerWeaponController().GetCurrentWeapon() != WeaponType.NONE && !Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.AIM);
            }
            else if (Owner.GetPlayerWeaponController().GetCurrentWeapon() == WeaponType.NONE)
            {
                stateMachine.ChangeState(PlayerActionState.NONE);
            }
        }
    }
}