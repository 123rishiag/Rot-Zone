using Game.Misc;
using UnityEngine;

namespace Game.Projectile
{
    public class RifleProjectileController : ProjectileController
    {
        public RifleProjectileController(ProjectileData _projectileData, Transform _parentPanel,
            Vector3 _firePosition, Vector3 _fireDirection, int _fireDistance,
            MiscService _miscService)
            : base(_projectileData, _parentPanel,
                  _firePosition, _fireDirection, _fireDistance,
                  _miscService)
        {
            // Using separate subcontrollers for each prefab to keep pooling simple, fast, and easy to manage.
        }
    }
}