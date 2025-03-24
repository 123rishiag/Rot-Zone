using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyAnimationController
    {
        // Private Variables
        private Animator enemyAnimator;
        private EnemyController enemyController;

        private readonly int moveXHash = Animator.StringToHash("moveX");
        private readonly int moveZHash = Animator.StringToHash("moveZ");

        public readonly int idleHash = Animator.StringToHash("Idle");
        public readonly int patrolHash = Animator.StringToHash("Patrol");
        public readonly int detectHash = Animator.StringToHash("Detect");

        public EnemyAnimationController(Animator _enemyAnimator, EnemyController _enemyController)
        {
            // Setting Variables
            enemyAnimator = _enemyAnimator;
            enemyController = _enemyController;
        }
    }
}