using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerModel
    {
        public PlayerModel(PlayerData _playerData)
        {
            WalkSpeed = _playerData.walkSpeed;
            RunSpeed = _playerData.runSpeed;
            AccelerationFactor = _playerData.accelerationFactor;
            DecelerationFactor = _playerData.decelerationFactor;
            DirectionSmoothSpeed = _playerData.directionSmoothSpeed;
            RotationSpeed = _playerData.rotationSpeed;

            GravityFactor = _playerData.gravityFactor;
            GroundCheckDistance = _playerData.groundCheckDistance;
            GroundLayer = _playerData.groundLayer;

            WeaponLayerWeightChangeFactor = _playerData.weaponLayerWeightChangeFactor;

            AimTransformDefaultPosition = _playerData.aimTransformDefaultPosition;
        }

        // Getters
        public float WalkSpeed { get; private set; }
        public float RunSpeed { get; private set; }
        public float AccelerationFactor { get; private set; }
        public float DecelerationFactor { get; private set; }
        public float DirectionSmoothSpeed { get; private set; }
        public float RotationSpeed { get; private set; }

        public float GravityFactor { get; private set; }
        public float GroundCheckDistance { get; private set; }
        public LayerMask GroundLayer { get; private set; }

        public float WeaponLayerWeightChangeFactor { get; private set; }

        public Vector3 AimTransformDefaultPosition { get; private set; }
    }
}