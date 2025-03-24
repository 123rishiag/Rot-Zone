using ServiceLocator.Projectile;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponController
    {
        // Private Variables
        private WeaponModel weaponModel;
        private WeaponView weaponView;

        private Vector3 cachedFirePosition;
        private Vector3 cachedFireDirection;
        private int currentAmmo;
        private int totalAmmoLeft;
        private float lastFireTime;

        // Private Services
        private ProjectileService projectileService;

        public WeaponController(WeaponData _weaponData, Transform _parentPanel,
            ProjectileService _projectileService)
        {
            // Setting Variables
            weaponModel = new WeaponModel(_weaponData);
            weaponView = Object.Instantiate(_weaponData.weaponPrefab, _parentPanel).GetComponent<WeaponView>();
            weaponView.Init(this);

            currentAmmo = weaponModel.WeaponInitialAmmo;
            totalAmmoLeft = weaponModel.WeaponTotalAmmo;
            lastFireTime = 0f;

            // Setting Services
            projectileService = _projectileService;
        }

        public void LateUpdate()
        {
            cachedFirePosition = weaponView.GetFirePoint().position;
            cachedFireDirection = weaponView.GetFirePoint().forward;
            weaponView.UpdateAimLaser();
        }

        public bool CanReloadWeapon()
        {
            return (currentAmmo < weaponModel.WeaponMaxCapacity && totalAmmoLeft > 0) ? true : false;
        }

        public bool CanFireWeapon()
        {
            return (lastFireTime == 0f ||
                (Time.time > lastFireTime + 1 / weaponModel.WeaponFireRateInSeconds)
                ) ? true : false;
        }

        public void ReloadWeapon()
        {
            int ammoInsertAvailable = weaponModel.WeaponMaxCapacity - currentAmmo;
            int ammoToAdd = Mathf.Min(totalAmmoLeft, ammoInsertAvailable);

            currentAmmo += ammoToAdd;
            totalAmmoLeft -= ammoToAdd;
        }

        public void FireWeapon()
        {
            if (currentAmmo > 0)
            {
                projectileService.FireProjectile(weaponModel.WeaponProjectileType, cachedFirePosition, cachedFireDirection);
                lastFireTime = Time.time;
                --currentAmmo;
            }
        }

        public void EnableWeapon() => weaponView.gameObject.SetActive(true);
        public void DisableWeapon() => weaponView.gameObject.SetActive(false);

        // Setters
        public void SetTransform(Transform _transform)
        {
            weaponView.transform.position = _transform.position;
            weaponView.transform.rotation = _transform.rotation;
            weaponView.transform.localScale = _transform.localScale;
        }

        // Getters
        public WeaponModel GetModel() => weaponModel;
        public WeaponView GetView() => weaponView;
    }
}