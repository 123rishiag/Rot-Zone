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

        [Header("Physics Settings")]
        [SerializeField] private Material normalConeMaterial;
        [SerializeField] private Material detectedConeMaterial;
        private Collider[] ragDollColliders;
        private Rigidbody[] ragDollRigidbodies;
        private TrailRenderer[] trailRenderers;

        private MeshFilter detectionMeshFilter;
        private MeshRenderer detectionMeshRenderer;
        private Mesh detectionMesh;

        public void Init(EnemyController _enemyController)
        {
            enemyController = _enemyController;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            ragDollColliders = GetComponentsInChildren<Collider>();
            ragDollRigidbodies = GetComponentsInChildren<Rigidbody>();
            trailRenderers = GetComponentsInChildren<TrailRenderer>();

            detectionMeshFilter = GetComponentInChildren<MeshFilter>();
            detectionMeshRenderer = detectionMeshFilter.GetComponent<MeshRenderer>();
            detectionMesh = new Mesh();
            detectionMeshFilter.mesh = detectionMesh;
        }

        public void HitImpactCoroutine(Vector3 _impactForce, int _damage, Collision _hitCollision)
        {
            StartCoroutine(enemyController.HitImpact(_impactForce, _damage, _hitCollision));
        }

        public void DrawDetectionCone()
        {
            if (enemyController == null || enemyController.GetEnemyStateMachine().GetCurrentState() == EnemyState.DEAD)
            {
                if (detectionMeshRenderer != null)
                    detectionMeshRenderer.enabled = false;

                return;
            }

            Vector3 origin = transform.position + Vector3.up;
            Vector3 forward = transform.forward;
            float detectionDistance = enemyController.DetectionDistance;
            float detectionAngleDegree = enemyController.GetModel().DetectionAngleDegree;

            // Cone mesh should be visible
            detectionMeshRenderer.enabled = true;

            // One segment per degree for smooth visual
            int segments = Mathf.RoundToInt(detectionAngleDegree);

            // Vertices: 1 for origin, and 1 for each segment point along the arc
            Vector3[] vertices = new Vector3[segments + 2];

            // Triangles: 3 indices per triangle
            int[] triangles = new int[segments * 3];

            // First vertex is the center/origin of the cone
            vertices[0] = Vector3.zero;

            // Generating vertices along the arc of the detection cone
            for (int i = 0; i <= segments; ++i)
            {
                // Stepping through the total detection angle, starting from the left edge (negative half-angle)
                // to the right edge (positive half-angle), one segment at a time
                float angle = -detectionAngleDegree / 2f + (detectionAngleDegree / segments) * i;

                // Rotating forward vector by the current angle around the Y-axis to get the direction of this segment
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 direction = rotation * Vector3.forward;

                // Calculating vertex position by extending the direction vector to detectionDistance
                // These are placed in local space, radiating outward from center
                vertices[i + 1] = direction * detectionDistance;
            }

            // Generating triangle indices to connect the center vertex to each pair of outer vertices
            for (int i = 0; i < segments; ++i)
            {
                // Triangle has 3 vertices:
                // - The center vertex (index 0)
                // - Current outer vertex (index i + 1)
                // - Next outer vertex (index i + 2)
                // This forms a "pizza slice" of the cone
                triangles[i * 3] = 0;           // Center of the cone
                triangles[i * 3 + 1] = i + 1;   // Current outer point
                triangles[i * 3 + 2] = i + 2;   // Next outer point (moves clockwise)
            }

            // Updating Mesh Transform to align with the enemy's orientation
            detectionMeshFilter.transform.position = origin;
            detectionMeshFilter.transform.rotation = Quaternion.LookRotation(forward);

            detectionMesh.Clear();
            detectionMesh.vertices = vertices;
            detectionMesh.triangles = triangles;
            detectionMesh.RecalculateNormals();
        }

        // Setters
        public void SetRagDollActive(bool _flag)
        {
            foreach (Rigidbody rb in ragDollRigidbodies)
            {
                rb.isKinematic = !_flag;
            }
        }
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
        public void SetConeDetectMaterial(bool _flag)
        {
            detectionMeshRenderer.material = _flag ? detectedConeMaterial : normalConeMaterial;
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
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;
    }
}
