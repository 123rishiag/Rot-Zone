using System;
using UnityEngine;

namespace ServiceLocator.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerView playerPrefab;
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        [Header("Movement Settings")]
        public float walkSpeed = 1.5f;
        public float runSpeed = 5f;
        public float accelerationFactor = 5f;
        public float decelerationFactor = 2.5f;
        public float directionSmoothSpeed = 10f;
        public float rotationSpeed = 3f;

        [Header("Gravity Settings")]
        public float gravityFactor = 9.81f;
        public float groundCheckDistance = 0.2f;
        public LayerMask groundLayer;

        [Header("Weapon Settings")]
        public float weaponLayerWeightChangeFactor = 10f;

        [Header("Aim Settings")]
        public Vector3 aimTransformDefaultPosition = new Vector3(0f, 1.5f, 1f);
    }
}