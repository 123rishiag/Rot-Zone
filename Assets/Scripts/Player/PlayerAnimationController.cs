using ServiceLocator.Weapon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerAnimationController
    {
        // Animation Parameters
        public int movementLocomotionHash = Animator.StringToHash("Movement Locomotion");
        public int fallHash = Animator.StringToHash("Fall");

        public int weaponFireHash = Animator.StringToHash("Weapon_Fire");
        public int weaponIdleHash = Animator.StringToHash("Weapon_Idle");

        private int moveXHash = Animator.StringToHash("moveX");
        private int moveZHash = Animator.StringToHash("moveZ");
        private int speedHash = Animator.StringToHash("speed");

        private int movementLayerIndex = 0;
        private int pistolLayerIndex = 1;
        private int rifleLayerIndex = 2;
        private int shotgunLayerIndex = 3;

        private float defaultIKWeight = 0.0f;
        private float pistolIKWeight = 1.0f;
        private float rifleIKWeight = 1.0f;
        private float shotgunIKWeight = 1.0f;

        // Private Variables
        private Animator playerAnimator;
        private PlayerController playerController;

        private float yawRotation;

        private int weaponAnimationLayerIndex;
        private float weaponAnimationLayerWeight;

        public PlayerAnimationController(Animator _playerAnimator, PlayerController _playerController)
        {
            // Setting Variables
            playerAnimator = _playerAnimator;
            playerController = _playerController;

            yawRotation = 0f;

            weaponAnimationLayerIndex = 0;
            weaponAnimationLayerWeight = 0f;
        }

        public void UpdateAnimation()
        {
            UpdateAnimationLayerWeight();
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

        public void UpdateAnimationLayerWeight()
        {
            if (weaponAnimationLayerIndex != -1) // Ensuring that the layer exists
            {
                float currentWeight = playerAnimator.GetLayerWeight(weaponAnimationLayerIndex);
                float targetWeight = Mathf.Lerp(currentWeight, weaponAnimationLayerWeight,
                    Time.deltaTime * playerController.GetModel().WeaponLayerWeightChangeFactor);
                playerAnimator.SetLayerWeight(weaponAnimationLayerIndex, targetWeight);
            }
        }

        // Setters
        public void SetAnimationLayer(WeaponType _weaponType)
        {
            int layerIndex = GetAnimationLayerIndex(_weaponType);
            for (int i = 1; i < playerAnimator.layerCount; ++i)
            {
                playerAnimator.SetLayerWeight(i, 0f);
            }

            weaponAnimationLayerIndex = layerIndex;
            weaponAnimationLayerWeight = 1f;
        }
        public void SetIKWeight(WeaponType _weaponType)
        {
            float weight = GetIKWeight(_weaponType);
            playerController.GetView().GetLeftHandIK().weight = weight;
            playerController.GetView().GetRightHandAimConstraint().weight = weight;
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
            switch (_weaponType)
            {
                case WeaponType.PISTOL:
                    return pistolIKWeight;
                case WeaponType.RIFLE:
                    return rifleIKWeight;
                case WeaponType.SHOTGUN:
                    return shotgunIKWeight;
                case WeaponType.NONE:
                default:
                    return defaultIKWeight;
            }
        }
    }
}