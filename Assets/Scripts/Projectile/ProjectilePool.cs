using ServiceLocator.Player;
using ServiceLocator.Utility;
using System;
using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        // Private Variables
        private ProjectileConfig projectileConfig;
        private Transform projectileParentPanel;

        private ProjectileType projectileType;
        private Transform firePoint;

        public ProjectilePool(ProjectileConfig _projectileConfig, Transform _parentPanel)
        {
            // Setting Variables
            projectileConfig = _projectileConfig;
            projectileParentPanel = _parentPanel;
        }

        public ProjectileController GetProjectile<T>(ProjectileType _projectileType, Transform _firePoint)
            where T : ProjectileController
        {
            // Setting Variables
            projectileType = _projectileType;
            firePoint = _firePoint;

            // Fetching Item
            var item = GetItem<T>();

            // Resetting Item Properties
            item.Reset(GetProjectileData(projectileType), firePoint);

            return item;
        }

        protected override ProjectileController CreateItem<T>()
        {
            // Creating Controller
            switch (projectileType)
            {
                case ProjectileType.PISTOL_PROJECTILE:
                    return new PistolProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePoint);
                case ProjectileType.RIFLE_PROJECTILE:
                    return new RifleProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePoint);
                case ProjectileType.SHOTGUN_PROJECTILE:
                    return new ShotgunProjectileController(
                        GetProjectileData(projectileType), projectileParentPanel, firePoint);
                default:
                    Debug.LogWarning($"Unhandled ProjectileType: {projectileType}");
                    return null;
            }
        }

        private ProjectileData GetProjectileData(ProjectileType _projectileType) =>
            Array.Find(projectileConfig.projectileData, w => w.projectileType == _projectileType);
    }
}