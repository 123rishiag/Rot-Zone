using ServiceLocator.Utility;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerActionReloadState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionReloadState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Disabling Current Fire Press
            Owner.IsFiring = false;

            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetWeaponController().GetCurrentWeaponType(), false);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponReloadHash);
        }
        public void Update()
        {
            CheckTransitionConditions();

            Owner.UpdateActionVariables();
        }

        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit()
        {
            Owner.GetWeaponController().ReloadComplete();
        }

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
            else if (IsReloadAnimationFinished())
            {
                Owner.GetWeaponController().ReloadWeapon();
                stateMachine.ChangeState(PlayerActionState.AIM);
            }
        }

        private bool IsReloadAnimationFinished()
        {
            int weaponLayerIndex = Owner.GetAnimationController().WeaponAnimationLayerIndex;
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(weaponLayerIndex);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().weaponReloadHash &&
                stateInfo.normalizedTime >= 0.9f);
        }
    }
}