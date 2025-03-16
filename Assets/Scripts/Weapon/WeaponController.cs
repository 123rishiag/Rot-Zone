using ServiceLocator.Projectile;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponController
    {
        // Private Variables
        private WeaponModel weaponModel;
        private WeaponView weaponView;

        private int currentAmmo;

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

            // Setting Services
            projectileService = _projectileService;
        }

        public void Update() => weaponView.UpdateAimLaser();

        public void FireWeapon()
        {
            if (currentAmmo > 0)
            {
                projectileService.FireProjectile(weaponModel.WeaponProjectileType, weaponView.GetFirePoint());
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