using Game.Utility;
using System;
using UnityEngine;

namespace Game.Projectile
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        // Private Variables
        private ProjectileConfig projectileConfig;
        private Transform projectileParentPanel;

        private ProjectileType projectileType;
        private Vector3 firePosition;
        private Vector3 fireDirection;

        public ProjectilePool(ProjectileConfig _projectileConfig, Transform _parentPanel)
        {
            // Setting Variables
            projectileConfig = _projectileConfig;
            projectileParentPanel = _parentPanel;
        }

        public ProjectileController GetProjectile<T>(ProjectileType _projectileType,
            Vector3 _firePosition, Vector3 _fireDirection)
            where T : ProjectileController
        {
            // Setting Variables
            projectileType = _projectileType;
            firePosition = _firePosition;
            fireDirection = _fireDirection;

            // Fetching Item
            var item = GetItem<T>();

            // Resetting Item Properties
            item.Reset(GetProjectileData(projectileType), firePosition, fireDirection);

            return item;
        }

        protected override ProjectileController CreateItem<T>()
        {
            // Creating Controller
            switch (projectileType)
            {
                case ProjectileType.PISTOL_PROJECTILE:
                    return new PistolProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePosition, fireDirection);
                case ProjectileType.RIFLE_PROJECTILE:
                    return new RifleProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePosition, fireDirection);
                case ProjectileType.SHOTGUN_PROJECTILE:
                    return new ShotgunProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePosition, fireDirection);
                default:
                    Debug.LogError($"Unhandled ProjectileType: {projectileType}");
                    return null;
            }
        }

        private ProjectileData GetProjectileData(ProjectileType _projectileType) =>
            Array.Find(projectileConfig.projectileData, w => w.projectileType == _projectileType);
    }
}