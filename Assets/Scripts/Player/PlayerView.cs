using ServiceLocator.Enemy;
using ServiceLocator.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ServiceLocator.Player
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
            if (enemyView != null)
            {
                Rigidbody enemyRigidbody = _collision.collider.attachedRigidbody;
                Vector3 impactForce = enemyRigidbody.linearVelocity.normalized *
                                              enemyView.GetController().GetModel().AttackForce;
                int damage = enemyView.GetController().GetModel().AttackDamage;
                HitImpactCoroutine(impactForce, damage, _collision);
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

        // Getters
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public Transform GetAimTransform() => aimTransform;
        public WeaponIKData[] GetWeaponIKDatas() => weaponIKDatas;
        public MultiAimConstraint GetRightHandAimConstraint() => rightHandAimConstraint;
        public TwoBoneIKConstraint GetLeftHandIK() => leftHandIK;
    }
}