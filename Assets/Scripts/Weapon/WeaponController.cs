using Game.Event;
using Game.Projectile;
using UnityEngine;

namespace Game.Weapon
{
    public abstract class WeaponController
    {
        // Private Variables
        private WeaponModel weaponModel;
        private WeaponView weaponView;

        private Vector3 cachedFirePosition;
        private Vector3 cachedFireDirection;
        private Vector3 aimTarget;

        private float lastFireTime;
        public int CurrentAmmo { get; private set; }
        public int TotalAmmoLeft { get; private set; }

        // Private Services
        protected EventService eventService;
        private ProjectileService projectileService;

        public WeaponController(WeaponData _weaponData, Transform _parentPanel,
            EventService _eventService, ProjectileService _projectileService)
        {
            // Setting Variables
            weaponModel = new WeaponModel(_weaponData);
            weaponView = Object.Instantiate(_weaponData.weaponPrefab, _parentPanel).GetComponent<WeaponView>();
            weaponView.Init(this);

            lastFireTime = 0f;
            CurrentAmmo = 0;
            TotalAmmoLeft = 0;

            // Setting Services
            eventService = _eventService;
            projectileService = _projectileService;
        }

        public void LateUpdate()
        {
            cachedFirePosition = weaponView.GetFirePoint().position;
            cachedFireDirection = weaponView.GetFirePoint().forward;
            weaponView.UpdateAimLaser(aimTarget);

            // For Debug
            Debug.DrawLine(cachedFirePosition, aimTarget, Color.green, 0.1f);
        }

        public void ReloadWeapon()
        {
            int ammoInsertAvailable = weaponModel.WeaponMaxCapacity - CurrentAmmo;
            int ammoToAdd = Mathf.Min(TotalAmmoLeft, ammoInsertAvailable);

            CurrentAmmo += ammoToAdd;
            TotalAmmoLeft -= ammoToAdd;
        }

        public void FireWeapon()
        {
            if (IsAmmoLeft())
            {
                projectileService.FireProjectile(weaponModel.WeaponProjectileType, cachedFirePosition, cachedFireDirection);
                --CurrentAmmo;
                PlayFireSound();
            }
            lastFireTime = Time.time;
        }
        public void UpdateWeaponAimPoint(Vector3 _target)
        {
            aimTarget = Vector3.Lerp(aimTarget, _target, Time.deltaTime * 50f);
        }

        public abstract void PlayFireSound();

        public void SetAmmo(int _ammoAmount)
        {
            CurrentAmmo = 0;
            TotalAmmoLeft = _ammoAmount;
            ReloadWeapon();
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
        public bool IsAmmoLeft() => CurrentAmmo > 0 ? true : false;
        public bool CanReloadWeapon()
        {
            return (CurrentAmmo < weaponModel.WeaponMaxCapacity && TotalAmmoLeft > 0) ? true : false;
        }
        public bool CanFireWeapon()
        {
            return (lastFireTime == 0f ||
                (Time.time > lastFireTime + 1 / weaponModel.WeaponFireRateInSeconds)
                ) ? true : false;
        }

        public bool IsEnabled() => weaponView.gameObject.activeSelf;
        public WeaponModel GetModel() => weaponModel;
        public WeaponView GetView() => weaponView;
    }
}