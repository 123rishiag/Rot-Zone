using ServiceLocator.Sound;
using ServiceLocator.Utility;
using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerActionFireState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionFireState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetWeaponController().FireWeapon();
            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetWeaponController().GetCurrentWeaponType(), true);
            if (!IsFireAnimationInProgress())
            {
                Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().weaponFireHash);
            }
            if (Owner.GetWeaponController().GetCurrentWeapon().CurrentAmmo == 0)
            {
                Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.WEAPON_EMPTY);
            }
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
            else if (Owner.GetWeaponController().GetCurrentWeapon().CanFireWeapon() && Owner.IsReloading)
            {
                stateMachine.ChangeState(PlayerActionState.RELOAD);
            }
            else if (Owner.GetWeaponController().GetCurrentWeapon().CanFireWeapon() && Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.FIRE);
            }
            else if (IsFireAnimationInProgress() && !Owner.IsFiring)
            {
                stateMachine.ChangeState(PlayerActionState.AIM);
            }
        }

        private bool IsFireAnimationInProgress()
        {
            int weaponLayerIndex = Owner.GetAnimationController().WeaponAnimationLayerIndex;
            AnimatorStateInfo stateInfo =
                Owner.GetView().GetAnimator().GetCurrentAnimatorStateInfo(weaponLayerIndex);

            return (stateInfo.shortNameHash == Owner.GetAnimationController().weaponFireHash &&
                stateInfo.normalizedTime >= 0.5f);
        }
    }
}