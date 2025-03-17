using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class PistolProjectileController : ProjectileController
    {
        public PistolProjectileController(ProjectileData _projectileData, Transform _parentPanel, Transform _firePoint)
            : base(_projectileData, _parentPanel, _firePoint)
        {

        }
    }
}