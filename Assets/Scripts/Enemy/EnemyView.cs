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

        public void Init(EnemyController _enemyController)
        {
            enemyController = _enemyController;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
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

        private void OnDrawGizmos()
        {
            DetectionGizmos();
        }
        private void DetectionGizmos()
        {
            float detectionDistance = enemyController.GetModel().DetectionDistance;
            float detectionAngleDegree = enemyController.GetModel().DetectionAngleDegree;

            Vector3 origin = transform.position + Vector3.up;

            int segments = 30; // Increase for smoother visualization
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

        // Getters
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;
    }
}
