using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class PistolProjectileController : ProjectileController
    {
        public PistolProjectileController(ProjectileData _projectileData, Transform _parentPanel,
            Vector3 _firePosition, Vector3 _fireDirection)
            : base(_projectileData, _parentPanel, _firePosition, _fireDirection)
        {

        }
    }
}