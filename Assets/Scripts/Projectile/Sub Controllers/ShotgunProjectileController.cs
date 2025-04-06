using UnityEngine;

namespace Game.Projectile
{
    public class ShotgunProjectileController : ProjectileController
    {
        public ShotgunProjectileController(ProjectileData _projectileData, Transform _parentPanel,
            Vector3 _firePosition, Vector3 _fireDirection)
            : base(_projectileData, _parentPanel, _firePosition, _fireDirection)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}