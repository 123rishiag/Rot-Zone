using Game.Event;
using Game.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class WeaponService
    {
        // Private Variables
        private WeaponConfig weaponConfig;

        private List<WeaponController> weaponControllers;

        // Private Services
        private EventService eventService;
        private ProjectileService projectileService;

        public WeaponService(WeaponConfig _weaponConfig)
        {
            // Setting Variables
            weaponConfig = _weaponConfig;

            weaponControllers = new List<WeaponController>();
        }

        public void Init(EventService _eventService, ProjectileService _projectileService)
        {
            // Setting Services
            eventService = _eventService;
            projectileService = _projectileService;
        }

        public void Update()
        {
            foreach (WeaponController weaponController in weaponControllers)
            {
                if(weaponController.IsEnabled())
                {
                    weaponController.Update();
                }
            }
        }
        public void LateUpdate()
        {
            foreach (WeaponController weaponController in weaponControllers)
            {
                if (weaponController.IsEnabled())
                {
                    weaponController.LateUpdate();
                }
            }
        }

        public WeaponController CreateWeapon(WeaponType _weaponType, Transform _parentPanel)
        {
            // Creating Controller
            WeaponController weaponController = null;
            switch (_weaponType)
            {
                case WeaponType.PISTOL:
                    weaponController = new PistolWeaponController(GetWeaponData(_weaponType), _parentPanel,
                        eventService, projectileService);
                    break;
                case WeaponType.RIFLE:
                    weaponController = new RifleWeaponController(GetWeaponData(_weaponType), _parentPanel,
                        eventService, projectileService);
                    break;
                case WeaponType.SHOTGUN:
                    weaponController = new ShotgunWeaponController(GetWeaponData(_weaponType), _parentPanel,
                        eventService, projectileService);
                    break;
                default:
                    Debug.LogError($"Unhandled Weapon Type: {_weaponType}");
                    return null;
            }

            weaponControllers.Add(weaponController);
            return weaponController;
        }

        // Getters
        private WeaponData GetWeaponData(WeaponType _weaponType) =>
            Array.Find(weaponConfig.weaponData, w => w.weaponType == _weaponType);
    }
}