using Game.Utility;
using Game.Weapon;

namespace Game.Player
{
    public class PlayerActionAimState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionAimState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetWeaponController().GetCurrentWeaponType(), true);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponIdleHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateActionVariables();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }

        private void CheckTransitionConditions()
        {
            if (Owner.GetWeaponController().GetCurrentWeaponType() == WeaponType.NONE)
            {
                stateMachine.ChangeState(PlayerActionState.NONE);
            }
            else if (Owner.GetWeaponController().GetCurrentWeapon().CanFireWeapon() && Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.FIRE);
            }
            else if (Owner.GetWeaponController().GetCurrentWeapon().CanReloadWeapon() && Owner.IsReloading)
            {
                stateMachine.ChangeState(PlayerActionState.RELOAD);
            }
        }
    }
}