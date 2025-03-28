using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerAnimationController
    {
        // Animation Parameters
        public readonly int movementLocomotionHash = Animator.StringToHash("Movement Locomotion");
        public readonly int fallHash = Animator.StringToHash("Fall");
        public readonly int hurtHash = Animator.StringToHash("Hurt");
        public readonly int deadHash = Animator.StringToHash("Dead");

        public readonly int weaponFireHash = Animator.StringToHash("Weapon_Fire");
        public readonly int weaponIdleHash = Animator.StringToHash("Weapon_Idle");
        public readonly int weaponReloadHash = Animator.StringToHash("Weapon_Reload");

        private readonly int moveXHash = Animator.StringToHash("moveX");
        private readonly int moveZHash = Animator.StringToHash("moveZ");
        private readonly int speedHash = Animator.StringToHash("speed");

        private readonly int movementLayerIndex = 0;
        private readonly int pistolLayerIndex = 1;
        private readonly int rifleLayerIndex = 2;
        private readonly int shotgunLayerIndex = 3;

        private readonly float weaponNotInActionIKWeight = 0.15f;
        private readonly float weaponInActionIKWeight = 1.0f;
        private readonly float noWeaponIKWeight = 0f;

        // Private Variables
        private Animator playerAnimator;
        private PlayerController playerController;

        private float yawRotation;

        public int WeaponAnimationLayerIndex { get; private set; }
        private float weaponAnimationLayerWeight;

        private float targetIKWeight;

        private bool isActionStateIKAllowed;

        public PlayerAnimationController(Animator _playerAnimator, PlayerController _playerController)
        {
            // Setting Variables
            playerAnimator = _playerAnimator;
            playerController = _playerController;

            yawRotation = 0f;

            WeaponAnimationLayerIndex = 0;
            weaponAnimationLayerWeight = 0f;

            targetIKWeight = 0f;

            isActionStateIKAllowed = false;
        }

        public void UpdateAnimation()
        {
            UpdateAnimationLayerWeight();
            UpdateIKWeight();
            UpdateAnimationParameters();
        }
        private void UpdateAnimationParameters()
        {
            float currentSpeed = playerController.GetCurrentSpeed();
            float walkSpeed = playerController.GetModel().WalkSpeed;
            float runSpeed = playerController.GetModel().RunSpeed;

            // We only want the player's horizontal rotation to affect animation, yaw means horizontal
            yawRotation = playerController.GetTransform().eulerAngles.y;
            Quaternion yawOnlyRotation = Quaternion.Euler(0, yawRotation, 0);

            // To convert world coordinates into the local coordinates of an object,
            // we multiply the world coordinates with the inverse of the object's localRotation.
            // To convert local coordinates of an object into world coordinates,
            // we multiply the local coordinates with the object's localRotation.
            Vector3 cameraRelativeMoveDir = Quaternion.Inverse(yawOnlyRotation) *
                playerController.GetMoveDirection();

            // Updating the "moveX" and "moveZ" parameter to smoothly blend between
            // forward, backward, left, right and diagonal movement in the Animator.
            playerAnimator.SetFloat(moveXHash, cameraRelativeMoveDir.x, 0.1f, Time.deltaTime);
            playerAnimator.SetFloat(moveZHash, cameraRelativeMoveDir.z, 0.1f, Time.deltaTime);

            float normalizedSpeed = 0f;

            //Mathf.InverseLerp(minRange, maxRange, value) means what percentage does values lies between 0 and 1.
            // Ex - minRange = 10, maxRange = 20, value = 15, result = .5;
            if (currentSpeed > 0f && currentSpeed <= walkSpeed)
            {
                // Ex - minRange = 0, maxRange = walkSpeed = 5, currentSpeed = 2.5, result = 0.5
                // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0) 
                normalizedSpeed = Mathf.InverseLerp(0f, walkSpeed, currentSpeed) * 0.5f + 0f;
            }
            else if (currentSpeed > walkSpeed)
            {
                // Ex - minRange = 5, maxRange = walkSpeed = 10, currentSpeed = 7.5, result = 0.5
                // Normalized Speed = 50% of result = 0.25 + base start for walkSpeed (which is 0.5) 
                normalizedSpeed = Mathf.InverseLerp(walkSpeed, runSpeed, currentSpeed) * 0.5f + 0.5f;
            }

            // Updating the "speed" parameter to smoothly blend between idle, walk, and run in the Animator.
            playerAnimator.SetFloat(speedHash, normalizedSpeed, 0.1f, Time.deltaTime);
        }

        private void UpdateAnimationLayerWeight()
        {
            if (WeaponAnimationLayerIndex != -1) // Ensuring that the layer exists
            {
                float currentWeight = playerAnimator.GetLayerWeight(WeaponAnimationLayerIndex);
                float targetWeight = Mathf.Lerp(currentWeight, weaponAnimationLayerWeight,
                    Time.deltaTime * playerController.GetModel().WeaponLayerWeightChangeFactor);
                playerAnimator.SetLayerWeight(WeaponAnimationLayerIndex, targetWeight);
            }
        }

        private void UpdateIKWeight()
        {
            float currentLeftHandIKWeight = playerController.GetView().GetLeftHandIK().weight;
            float currentRightHandConstraintWeight = playerController.GetView().GetRightHandAimConstraint().weight;

            playerController.GetView().GetLeftHandIK().weight =
                Mathf.Lerp(currentLeftHandIKWeight, targetIKWeight, Time.deltaTime * 10f);
            playerController.GetView().GetRightHandAimConstraint().weight =
                Mathf.Lerp(currentRightHandConstraintWeight, targetIKWeight, Time.deltaTime * 10f);
        }

        // Setters
        public void SetAnimationLayer(WeaponType _weaponType)
        {
            int layerIndex = GetAnimationLayerIndex(_weaponType);
            for (int i = 1; i < playerAnimator.layerCount; ++i)
            {
                playerAnimator.SetLayerWeight(i, 0f);
            }

            WeaponAnimationLayerIndex = layerIndex;
            weaponAnimationLayerWeight = 1f;
        }
        public void SetIKWeight(WeaponType _weaponType)
        {
            float weight = GetIKWeight(_weaponType);
            targetIKWeight = weight;
        }

        public void EnableIKWeight(WeaponType _weaponType, bool _flag)
        {
            isActionStateIKAllowed = _flag;
            SetIKWeight(_weaponType);
        }

        // Getters
        private int GetAnimationLayerIndex(WeaponType _weaponType)
        {
            switch (_weaponType)
            {
                case WeaponType.PISTOL:
                    return pistolLayerIndex;
                case WeaponType.RIFLE:
                    return rifleLayerIndex;
                case WeaponType.SHOTGUN:
                    return shotgunLayerIndex;
                case WeaponType.NONE:
                default:
                    return movementLayerIndex;
            }
        }
        private float GetIKWeight(WeaponType _weaponType)
        {
            if (_weaponType == WeaponType.NONE)
                return noWeaponIKWeight;
            else if (_weaponType != WeaponType.NONE && !isActionStateIKAllowed)
                return weaponNotInActionIKWeight;
            else
                return weaponInActionIKWeight;
        }
    }
}