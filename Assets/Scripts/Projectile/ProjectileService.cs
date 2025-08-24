using Game.Misc;
using UnityEngine;

namespace Game.Projectile
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

        public void Init(MiscService _miscService)
        {
            // Setting Variables
            projectilePool = new ProjectilePool(projectileConfig, projectileParentPanel, _miscService);
        }
        public void Reset()
        {
            DestroyProjectiles(true);
        }

        public void Update()
        {
            DestroyProjectiles(false);
        }
        private void DestroyProjectiles(bool _isAllFlag)
        {
            for (int i = projectilePool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's IsUsed is false
                if (!projectilePool.pooledItems[i].IsUsed && !_isAllFlag)
                {
                    continue;
                }

                var projectileController = projectilePool.pooledItems[i].Item;
                if (!projectileController.IsActive() || _isAllFlag)
                {
                    ReturnProjectileToPool(projectileController);
                }
            }
        }

        public ProjectileController FireProjectile(ProjectileType _projectileType,
            Vector3 _firePosition, Vector3 _fireDirection, int _fireDistance)
        {
            switch (_projectileType)
            {
                case ProjectileType.PISTOL_PROJECTILE:
                    return projectilePool.GetProjectile<PistolProjectileController>(_projectileType,
                        _firePosition, _fireDirection, _fireDistance);
                case ProjectileType.RIFLE_PROJECTILE:
                    return projectilePool.GetProjectile<RifleProjectileController>(_projectileType,
                        _firePosition, _fireDirection, _fireDistance);
                case ProjectileType.SHOTGUN_PROJECTILE:
                    return projectilePool.GetProjectile<ShotgunProjectileController>(_projectileType,
                        _firePosition, _fireDirection, _fireDistance);
                default:
                    Debug.LogError($"Unhandled ProjectileType: {_projectileType}");
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