using System;
using UnityEngine;

namespace ServiceLocator.Enemy
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
        public int maxHealth = 3;
        public float idleDuration = 3f;
        public float patrolSpeed = 2f;
        public float patrolMaxRadius = 20f;
        public float rotationSpeed = 180;
        public float chaseSpeed = 5f;
        public float detectionMinDistance = 1f;
        public float detectionMaxDistance = 10f;
        public float detectionAngleDegree = 120f;
        public float detectionMinScreamDistance = 5f;
        public float stopDistance = 1f;
        public bool isGizmosEnabled = false;
    }
}