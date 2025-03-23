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
        public float moveSpeed = 10f;
    }
}