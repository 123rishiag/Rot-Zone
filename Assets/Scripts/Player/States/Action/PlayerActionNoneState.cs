using Game.Utility;
using Game.Weapon;

namespace Game.Player
{
    public class PlayerActionNoneState<T> : IState<PlayerController, PlayerActionState>
    {
        public PlayerController Owner { get; set; }
        private PlayerActionStateMachine stateMachine;

        public PlayerActionNoneState(PlayerActionStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetAnimationController().EnableIKWeight(
                Owner.GetWeaponController().GetCurrentWeaponType(), true);
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
            if (Owner.GetWeaponController().GetCurrentWeaponType() != WeaponType.NONE)
            {
                stateMachine.ChangeState(PlayerActionState.AIM);
            }
        }
    }
}