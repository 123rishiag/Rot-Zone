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
            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetWeaponVisualController().GetCurrentWeapon(), false);
            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponReloadHash);
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
            int weaponLayerIndex = Owner.GetAnimationController().WeaponAnimationLayerIndex;
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(weaponLayerIndex);

            if (Owner.GetWeaponVisualController().GetCurrentWeapon() != WeaponType.NONE && Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.FIRE);
            }
            else if (stateInfo.shortNameHash == Owner.GetAnimationController().weaponReloadHash &&
                stateInfo.normalizedTime >= 0.9f)
            {
                stateMachine.ChangeState(PlayerActionState.AIM);
            }
        }
    }
}