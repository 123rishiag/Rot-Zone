using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyDeadState<T> : IState<EnemyController, EnemyState>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float deadTimer;
        private const float hideDuration = 3f;

        public EnemyDeadState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            deadTimer = 0f;

            var enemyModel = Owner.GetModel();

            Owner.GetView().GetAnimator().enabled = false;
            Owner.GetView().SetRagDollActive(true);
            Owner.GetView().GetCharacterController().enabled = false;
            Owner.GetView().GetNavMeshAgent().enabled = false;

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().deadHash);
            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.ENEMY_DEAD);
        }

        public void Update()
        {
            deadTimer += Time.deltaTime;
            if (deadTimer >= hideDuration)
            {
                Owner.GetView().HideView();
            }
        }
        public void FixedUpdate() { }
        public void LateUpdate() { }
        public void OnStateExit() { }
    }
}