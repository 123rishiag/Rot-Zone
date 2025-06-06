using Game.Enemy;
using Game.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Game.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private WeaponIKData[] weaponIKDatas;
        [SerializeField] private MultiAimConstraint rightHandAimConstraint;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;

        [Header("Aim Settings")]
        [SerializeField] private Transform aimTransform;

        [Header("Physics Settings")]
        private Collider[] ragDollColliders;
        private Rigidbody[] ragDollRigidbodies;

        // Private Variables
        private PlayerController playerController;
        private CharacterController characterController;
        private Animator animator;

        public void Init(PlayerController _playerController)
        {
            // Setting Variables
            playerController = _playerController;
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();

            ragDollColliders = GetComponentsInChildren<Collider>();
            ragDollRigidbodies = GetComponentsInChildren<Rigidbody>();

            SetRagDollActive(true);
        }

        public void HitImpactCoroutine(Vector3 _impactForce, int _damage, Collision _hitCollision)
        {
            StopAllCoroutines();
            StartCoroutine(playerController.HitImpact(_impactForce, _damage, _hitCollision));
        }

        private void OnCollisionEnter(Collision _collision)
        {
            EnemyView enemyView = _collision.collider.GetComponentInParent<EnemyView>();
            if (enemyView != null &&
                enemyView.GetController().GetEnemyStateMachine().GetCurrentState() != EnemyState.DEAD &&
                playerController.GetMovementStateMachine().GetCurrentState() != PlayerMovementState.DEAD)
            {
                Rigidbody enemyRigidbody = _collision.collider.attachedRigidbody;
                Vector3 impactForce = enemyRigidbody.linearVelocity.normalized *
                                              enemyView.GetController().GetModel().AttackForce;
                int damage = enemyView.GetController().GetModel().AttackDamage;
                HitImpactCoroutine(impactForce, damage, _collision);
            }
        }

        // Debug Circle for Aiming
        public void DrawDebugCircle(Vector3 _center, Vector3 _normal, float _radius, int _segments = 32)
        {
            // Creating the first axis perpendicular to the normal (in the plane of the circle)
            Vector3 axisA = Vector3.Cross(_normal, Vector3.up);

            // If normal is nearly parallel to Vector3.up, using Vector3.right to avoid zero vector
            if (axisA.sqrMagnitude < 0.001f)
                axisA = Vector3.Cross(_normal, Vector3.right);

            axisA.Normalize();

            // Creating the second axis perpendicular to both normal and axisA (also lies in circle plane)
            Vector3 axisB = Vector3.Cross(_normal, axisA).normalized;

            // Start point on the circle edge
            Vector3 prevPoint = _center + axisA * _radius;

            // Drawing all the segments
            for (int i = 1; i <= _segments; ++i)
            {
                // Angle for this segment
                float angle = (i * Mathf.PI * 2f) / _segments;

                // Calculating next point using cosine and sine on the two axes
                Vector3 nextPoint = _center + (axisA * Mathf.Cos(angle) + axisB * Mathf.Sin(angle)) * _radius;

                // Drawing line between previous and next point
                Debug.DrawLine(prevPoint, nextPoint, Color.red, 0.1f);

                // Moving to the next point
                prevPoint = nextPoint;
            }
        }

        // Setters
        public void SetRagDollActive(bool _flag)
        {
            foreach (Rigidbody rb in ragDollRigidbodies)
            {
                rb.isKinematic = !_flag;
            }
        }
        public void SetPosition(Vector3 _spawnPosition)
        {
            transform.position = _spawnPosition;
        }

        // Getters
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public Transform GetAimTransform() => aimTransform;
        public WeaponIKData[] GetWeaponIKDatas() => weaponIKDatas;
        public MultiAimConstraint GetRightHandAimConstraint() => rightHandAimConstraint;
        public TwoBoneIKConstraint GetLeftHandIK() => leftHandIK;

    }
}