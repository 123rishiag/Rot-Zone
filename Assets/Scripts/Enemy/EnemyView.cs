using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        // Private Variables
        private EnemyController enemyController;
        private CharacterController characterController;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        [Header("Physics Settings")]
        private TrailRenderer[] trailRenderers;

        public void Init(EnemyController _enemyController)
        {
            enemyController = _enemyController;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            trailRenderers = GetComponentsInChildren<TrailRenderer>();
        }

        public void HitImpact(int _damage)
        {
            enemyController.HitImpact(_damage);
        }

        // Setters
        public void StopNavMeshAgent(bool _flag)
        {
            if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.isStopped = _flag;
                if (_flag)
                {
                    navMeshAgent.velocity = Vector3.zero;
                }
            }
        }
        public void SetTrailRenderActive(bool _flag)
        {
            foreach (TrailRenderer tr in trailRenderers)
            {
                tr.Clear();
                tr.enabled = _flag;
            }
        }
        public void SetPosition(Vector3 _spawnPosition)
        {
            transform.position = _spawnPosition;
        }
        public void ShowView()
        {
            gameObject.SetActive(true);
        }
        public void HideView()
        {
            gameObject.SetActive(false);
        }

        // Getters
        public EnemyController GetController() => enemyController;
        public Animator GetAnimator() => animator;
        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;
    }
}
