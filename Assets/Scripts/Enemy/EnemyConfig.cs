using System;
using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public EnemyData[] enemyData;
    }

    [Serializable]
    public class EnemyData
    {
        [Header("Enemy Details")]
        public EnemyType enemyType;
        public EnemyView enemyPrefab;

        [Header("Health Settings")]
        public int maxHealth = 10;

        [Header("Movement Settings")]
        public float idleDuration = 3f;
        public float patrolSpeed = 2f;
        public float patrolMaxRadius = 20f;
        public float rotationSpeed = 180;
        public float chaseSpeed = 5f;

        [Header("Attack Settings")]
        public float attackForce = 3f;
        public int attackDamage = 1;

        [Header("Detection Settings")]
        public float detectionMinDistance = 1f;
        public float detectionMaxDistance = 10f;
        public float detectionMinScreamDistance = 5f;
        public float stopDistance = 1f;

        [Header("Death Settings")]
        public float deathSlowDownTimeInSeconds = 2f;
    }
}