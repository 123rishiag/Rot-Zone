using System;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public GameObject CreateWeapon(WeaponType _weaponType)
        {
            GameObject weaponPrefab = GetWeaponData(_weaponType).weaponPrefab;
            GameObject weapon = Object.Instantiate(weaponPrefab);
            return weapon;
        }

        private WeaponData GetWeaponData(WeaponType _weaponType) =>
            Array.Find(weaponConfig.weaponData, w => w.weaponType == _weaponType);
    }
}