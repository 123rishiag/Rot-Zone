using UnityEngine;
using UnityEngine.AI;

namespace ServiceLocator.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        // Private Variables
        private EnemyController enemyController;
        private CharacterController characterController;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private Collider[] ragDollColliders;
        private Rigidbody[] ragDollRigidbodies;
        private TrailRenderer[] trailRenderers;

        public void Init(EnemyController _enemyController)
        {
            enemyController = _enemyController;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            ragDollColliders = GetComponentsInChildren<Collider>();
            ragDollRigidbodies = GetComponentsInChildren<Rigidbody>();
            trailRenderers = GetComponentsInChildren<TrailRenderer>();

            SetRagDollActive(true);
            SetTrailRenderActive(false);
        }

        public void HitImpactCoroutine(Vector3 _impactForce, Collision _hitCollision)
        {
            StopAllCoroutines();
            StartCoroutine(enemyController.HitImpact(_impactForce, _hitCollision));
        }

        private void OnDrawGizmos()
        {
            if (enemyController == null || transform == null)
                return;

            if (enemyController.GetModel().IsGizmosEnabled)
            {
                DetectionGizmos();
            }
        }
        private void DetectionGizmos()
        {
            float detectionDistance = enemyController.GetModel().DetectionMaxDistance;
            float detectionAngleDegree = enemyController.GetModel().DetectionAngleDegree / 2f;

            Vector3 origin = transform.position + Vector3.up;

            int segments = Mathf.RoundToInt(detectionAngleDegree);
            float angleStep = detectionAngleDegree * 2 / segments;

            Gizmos.color = Color.yellow;

            Vector3 previousDir = Quaternion.Euler(0, -detectionAngleDegree, 0) * transform.forward;

            // Drawing initial radial line
            Gizmos.DrawLine(origin, origin + previousDir * detectionDistance);

            // Drawing lines forming an arc
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -detectionAngleDegree + angleStep * i;
                Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

                // Arc edge
                Gizmos.DrawLine(origin + previousDir * detectionDistance, origin + currentDir * detectionDistance);

                // Radial line
                Gizmos.DrawLine(origin, origin + currentDir * detectionDistance);

                previousDir = currentDir;
            }

            // Drawing central forward direction line for clarity
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + transform.forward * detectionDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, 0.25f);
        }
        // Setters
        public void SetRagDollActive(bool _flag)
        {
            foreach (Rigidbody rb in ragDollRigidbodies)
            {
                rb.isKinematic = !_flag;
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
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;
    }
}
