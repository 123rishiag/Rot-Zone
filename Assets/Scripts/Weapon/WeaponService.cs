using System;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponService
    {
        // Private Variables
        private WeaponConfig weaponConfig;

        public WeaponService(WeaponConfig _weaponConfig)
        {
            // Setting Variables
            weaponConfig = _weaponConfig;
        }

        public WeaponController CreateWeapon(WeaponType _weaponType, Transform _parentPanel)
        {
            return new WeaponController(GetWeaponData(_weaponType).weaponPrefab, _parentPanel);
        }

        private WeaponData GetWeaponData(WeaponType _weaponType) =>
            Array.Find(weaponConfig.weaponData, w => w.weaponType == _weaponType);
    }
}