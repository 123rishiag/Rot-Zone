using System;
using UnityEngine;

namespace ServiceLocator.Projectile
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
    }
}