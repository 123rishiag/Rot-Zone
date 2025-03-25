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
        public EnemyType enemyType;
        public EnemyView enemyPrefab;
        public float idleDuration = 3f;
        public float patrolSpeed = 2f;
        public float patrolMaxRadius = 20f;
        public float patrolStopDistance = 1f;
        public float rotationSpeed = 180;
        public float detectionDistance = 10f;
        public float detectionAngleDegree = 120f;
        public float chaseSpeed = 5f;
        public float stopDistance = 1.5f;
    }
}