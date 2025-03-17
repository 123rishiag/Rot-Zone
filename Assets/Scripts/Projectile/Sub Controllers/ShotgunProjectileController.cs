using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ShotgunProjectileController : ProjectileController
    {
        public ShotgunProjectileController(ProjectileData _projectileData, Transform _parentPanel, Transform _firePoint)
            : base(_projectileData, _parentPanel, _firePoint)
        {

        }
    }
}