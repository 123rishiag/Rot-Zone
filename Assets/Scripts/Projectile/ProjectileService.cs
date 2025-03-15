using System;
using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileService
    {
        // Private Variables
        private ProjectileConfig projectileConfig;
        private Transform projectileParentPanel;

        public ProjectileService(ProjectileConfig _projectileConfig, Transform _parentPanel)
        {
            // Setting Variables
            projectileConfig = _projectileConfig;
            projectileParentPanel = _parentPanel;
        }

        public ProjectileController FireProjectile(ProjectileType _projectileType, Transform _firePoint)
        {
            return new ProjectileController(GetProjectileData(_projectileType), projectileParentPanel, _firePoint);
        }

        private ProjectileData GetProjectileData(ProjectileType _projectileType) =>
            Array.Find(projectileConfig.projectileData, w => w.projectileType == _projectileType);
    }
}