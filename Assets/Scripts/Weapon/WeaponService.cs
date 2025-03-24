using ServiceLocator.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponService
    {
        // Private Variables
        private WeaponConfig weaponConfig;

        private List<WeaponController> weaponControllers;

        // Private Services
        private ProjectileService projectileService;

        public WeaponService(WeaponConfig _weaponConfig)
        {
            // Setting Variables
            weaponConfig = _weaponConfig;

            weaponControllers = new List<WeaponController>();
        }

        public void Init(ProjectileService _projectileService)
        {
            // Setting Services
            projectileService = _projectileService;
        }

        public void LateUpdate()
        {
            foreach (WeaponController weaponController in weaponControllers)
            {
                weaponController.LateUpdate();
            }
        }

        public WeaponController CreateWeapon(WeaponType _weaponType, Transform _parentPanel)
        {
            WeaponController weaponController = new WeaponController(GetWeaponData(_weaponType), _parentPanel,
                projectileService);

            weaponControllers.Add(weaponController);

            return weaponController;
        }

        // Getters
        private WeaponData GetWeaponData(WeaponType _weaponType) =>
            Array.Find(weaponConfig.weaponData, w => w.weaponType == _weaponType);
    }
}