using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyStunState : IState<EnemyController>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float stunTimer;
        private const float recoveryDuration = 2f;

        public EnemyStunState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            stunTimer = 0f;

            Owner.GetView().StopNavMeshAgent(true);

            Owner.GetView().GetAnimator().CrossFade(Owner.GetAnimationController().stunHash, 0.5f);
        }
        public void Update()
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= recoveryDuration)
            {
                stateMachine.ChangeState(EnemyState.DETECT);
            }
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}