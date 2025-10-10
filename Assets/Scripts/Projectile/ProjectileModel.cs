using UnityEngine;

namespace Game.Projectile
{
    public class ProjectileModel
    {
        public ProjectileModel(ProjectileData _projectileData)
        {
            Reset(_projectileData);
        }

        public void Reset(ProjectileData _projectileData)
        {
            ProjectileType = _projectileData.projectileType;

            ProjectileSpeed = _projectileData.projectileSpeed;
            ProjectileDamage = _projectileData.projectileDamage;

            CollisionDamageLayerMask = _projectileData.collisionDamageLayerMask;
            CollisionDestroyLayerMask = _projectileData.collisionDestroyLayerMask;
        }

        // Getters
        public ProjectileType ProjectileType { get; private set; }

        public float ProjectileSpeed { get; private set; }
        public int ProjectileDamage { get; private set; }

        public LayerMask CollisionDamageLayerMask { get; private set; }
        public LayerMask CollisionDestroyLayerMask { get; private set; }
    }
}