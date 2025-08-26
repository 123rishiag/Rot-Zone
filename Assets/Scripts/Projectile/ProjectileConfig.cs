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

        [Header("Projectile Feature Settings")]
        [Range(100, 1000)]
        public float projectileSpeed = 300f;
        public float projectileImpactForce = 10f;
        public int projectileDamage = 1;

        [Header("Projectile Collision Settings")]
        public LayerMask collisionDamageLayerMask;
        public LayerMask collisionDestroyLayerMask;
    }
}