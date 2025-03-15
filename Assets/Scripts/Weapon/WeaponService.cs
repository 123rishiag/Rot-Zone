using ServiceLocator.Projectile;
using System;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponService
    {
        // Private Variables
        private WeaponConfig weaponConfig;

        // Private Services
        private ProjectileService projectileService;

        public WeaponService(WeaponConfig _weaponConfig)
        {
            // Setting Variables
            weaponConfig = _weaponConfig;
        }

        public void Init(ProjectileService _projectileService)
        {
            // Setting Services
            projectileService = _projectileService;
        }

        public WeaponController CreateWeapon(WeaponType _weaponType, Transform _parentPanel)
        {
            return new WeaponController(GetWeaponData(_weaponType), _parentPanel,
                projectileService);
        }

        // Getters
        private WeaponData GetWeaponData(WeaponType _weaponType) =>
            Array.Find(weaponConfig.weaponData, w => w.weaponType == _weaponType);
    }
}