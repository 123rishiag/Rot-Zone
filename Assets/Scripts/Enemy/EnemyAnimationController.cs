using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAnimationController
    {
        // Private Variables
        private Animator enemyAnimator;
        private EnemyController enemyController;

        public readonly int idleHash = Animator.StringToHash("Idle");
        public readonly int patrolHash = Animator.StringToHash("Patrol");
        public readonly int detectHash = Animator.StringToHash("Detect");
        public readonly int chaseHash = Animator.StringToHash("Chase");
        public readonly int attackHash = Animator.StringToHash("Attack");
        public readonly int hurtHash = Animator.StringToHash("Hurt");
        public readonly int stunHash = Animator.StringToHash("Stun");
        public readonly int deadHash = Animator.StringToHash("Idle");

        public EnemyAnimationController(Animator _enemyAnimator, EnemyController _enemyController)
        {
            // Setting Variables
            enemyAnimator = _enemyAnimator;
            enemyController = _enemyController;
        }
    }
}