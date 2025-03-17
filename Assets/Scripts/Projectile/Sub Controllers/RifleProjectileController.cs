using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class RifleProjectileController : ProjectileController
    {
        public RifleProjectileController(ProjectileData _projectileData, Transform _parentPanel, Transform _firePoint)
            : base(_projectileData, _parentPanel, _firePoint)
        {

        }
    }
}