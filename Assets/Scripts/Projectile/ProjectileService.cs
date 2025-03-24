using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileService
    {
        // Private Variables
        private ProjectileConfig projectileConfig;
        private Transform projectileParentPanel;

        private ProjectilePool projectilePool;

        public ProjectileService(ProjectileConfig _projectileConfig, Transform _parentPanel)
        {
            // Setting Variables
            projectileConfig = _projectileConfig;
            projectileParentPanel = _parentPanel;
        }

        public void Init()
        {
            // Setting Variables
            projectilePool = new ProjectilePool(projectileConfig, projectileParentPanel);
        }

        public void Update()
        {
            DestroyProjectiles();
        }
        private void DestroyProjectiles()
        {
            for (int i = projectilePool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's isUsed is false
                if (!projectilePool.pooledItems[i].isUsed)
                {
                    continue;
                }

                var projectileController = projectilePool.pooledItems[i].Item;
                if (!projectileController.IsActive())
                {
                    ReturnProjectileToPool(projectileController);
                }
            }
        }

        public ProjectileController FireProjectile(ProjectileType _projectileType, Vector3 _firePosition, Vector3 _fireDirection)
        {
            switch (_projectileType)
            {
                case ProjectileType.PISTOL_PROJECTILE:
                    return projectilePool.GetProjectile<PistolProjectileController>(_projectileType, _firePosition, _fireDirection);
                case ProjectileType.RIFLE_PROJECTILE:
                    return projectilePool.GetProjectile<RifleProjectileController>(_projectileType, _firePosition, _fireDirection);
                case ProjectileType.SHOTGUN_PROJECTILE:
                    return projectilePool.GetProjectile<ShotgunProjectileController>(_projectileType, _firePosition, _fireDirection);
                default:
                    Debug.LogWarning($"Unhandled ProjectileType: {_projectileType}");
                    return null;
            }
        }

        private void ReturnProjectileToPool(ProjectileController _projectileToReturn)
        {
            _projectileToReturn.GetView().HideView();
            projectilePool.ReturnItem(_projectileToReturn);
        }
    }
}