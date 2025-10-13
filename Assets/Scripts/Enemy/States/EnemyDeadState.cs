using Game.Sound;
using Game.Utility;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyDeadState : IState<EnemyController>
    {
        public EnemyController Owner { get; set; }
        private EnemyStateMachine stateMachine;

        private float deadTimer;
        private const float hideDuration = 3f;

        public EnemyDeadState(EnemyStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            deadTimer = 0f;

            Owner.GetView().StopNavMeshAgent(true);

            Owner.GetView().GetAnimator().Play(Owner.GetAnimationController().deadHash);
            Owner.EventService.OnPlaySoundEffectEvent.Invoke(SoundType.ENEMY_DEAD);


            Owner.MiscService.GetController().StartSlowDownCoroutine(Owner.GetModel().DeathSlowDownTimeInSeconds);
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