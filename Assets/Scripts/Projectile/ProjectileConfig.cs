using System;
using UnityEngine;

namespace Game.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Scriptable Objects/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [Header("Projectile Settings")]
        public ProjectileData[] projectileData;
    }

    [Serializable]
    public class ProjectileData
    {
        public ProjectileType projectileType;
        public ProjectileView projectilePrefab;
        public float projectileSpeed = 10f;
        public float projectileForce = 10f;
        public int projectileDamage = 1;
        public float projectileNoActivityDisableTime = 5f;
    }
}