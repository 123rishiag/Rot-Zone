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

        // Private Variables
        private CharacterController characterController;
        private Animator animator;

        public void Init()
        {
            // Setting Variables
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        #region Getters
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public Transform GetAimTransform() => aimTransform;
        public WeaponIKData[] GetWeaponIKDatas() => weaponIKDatas;
        public MultiAimConstraint GetRightHandAimConstraint() => rightHandAimConstraint;
        public TwoBoneIKConstraint GetLeftHandIK() => leftHandIK;
        #endregion
    }
}