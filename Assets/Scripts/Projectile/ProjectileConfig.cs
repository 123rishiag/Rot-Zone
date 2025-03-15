using UnityEngine;

namespace ServiceLocator.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Scriptable Objects/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [Header("Projectile Settings")]
        public ProjectileType projectileType;
        public ProjectileView projectilePrefab;
    }
}