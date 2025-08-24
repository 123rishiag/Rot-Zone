using Game.Event;
using Game.Projectile;
using Game.Utility;
using System.Collections;
using UnityEngine;

namespace Game.Weapon
{
    public abstract class WeaponController
    {
        // Private Variables
        private WeaponModel weaponModel;
        private WeaponView weaponView;

        private LayerMask aimLayer;

        private Vector3 cachedFirePosition;
        private Vector3 cachedFireDirection;
        private Vector3 aimTarget;

        private float lastFireTime;
        public int CurrentAmmo { get; private set; }
        public int TotalAmmoLeft { get; private set; }

        private WaitForSeconds gapInSingleShotYield;

        // Private Services
        protected EventService EventService { get; private set; }
        private MiscService miscService;
        private ProjectileService projectileService;

        public WeaponController(WeaponData _weaponData, Transform _parentPanel,
            EventService _eventService, MiscService _miscService, ProjectileService _projectileService)
        {
            // Setting Variables
            weaponModel = new WeaponModel(_weaponData);
            weaponView = Object.Instantiate(_weaponData.weaponPrefab, _parentPanel).GetComponent<WeaponView>();
            weaponView.Init(this);

            // Setting Services
            EventService = _eventService;
            miscService = _miscService;
            projectileService = _projectileService;

            // Setting Elements
            lastFireTime = 0f;
            CurrentAmmo = 0;
            TotalAmmoLeft = 0;

            gapInSingleShotYield = new WaitForSeconds(weaponModel.WeaponAmmoGapInSingleShot);
        }

        public void LateUpdate()
        {
            cachedFirePosition = weaponView.GetFirePoint().position;
            cachedFireDirection = (aimTarget - cachedFirePosition).normalized;
            UpdateAimLaser();
        }

        private void UpdateAimLaser()
        {
            Vector3 hitPoint;
            if (Physics.Raycast(cachedFirePosition, cachedFireDirection, out RaycastHit hit,
                weaponModel.WeaponRangeDistanceInMeters, aimLayer))
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = cachedFirePosition + cachedFireDirection * weaponModel.WeaponRangeDistanceInMeters;
            }
            weaponView.UpdateAimLaser(hitPoint);

            // For Debug
            // Debug.DrawLine(cachedFirePosition, hitPoint, Color.cyan, 0.1f);
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
            int consecutiveFireAmmo = Mathf.Min(CurrentAmmo, weaponModel.WeaponAmmoCountInSingleShot);
            PerformFire(consecutiveFireAmmo);
            CurrentAmmo -= consecutiveFireAmmo;
            lastFireTime = Time.time;
        }
        private void PerformFire(int _consecutiveFireAmmo)
        {
            miscService.StartManualCoroutine(BurstFire(_consecutiveFireAmmo));
        }
        private IEnumerator BurstFire(int _consecutiveFireAmmo)
        {
            for (int i = 0; i < _consecutiveFireAmmo; ++i)
            {
                FireSingleShot();
                yield return gapInSingleShotYield;
            }
        }
        private void FireSingleShot()
        {
            Vector3 direction = (cachedFireDirection + new Vector3(
                    Random.Range(-weaponModel.WeaponSpreadFactor, weaponModel.WeaponSpreadFactor),
                    Random.Range(-weaponModel.WeaponSpreadFactor, weaponModel.WeaponSpreadFactor),
                    Random.Range(-weaponModel.WeaponSpreadFactor, weaponModel.WeaponSpreadFactor)
               )).normalized;
            projectileService.FireProjectile(weaponModel.WeaponProjectileType,
                cachedFirePosition, direction, weaponModel.WeaponRangeDistanceInMeters);
            PlayFireSound();
        }

        public void UpdateWeaponAimPoint(Vector3 _target)
        {
            aimTarget = Vector3.Lerp(aimTarget, _target, Time.deltaTime * weaponModel.WeaponAimSpeed);
        }

        public abstract void PlayFireSound();

        public void SetAmmo(int _ammoAmount)
        {
            CurrentAmmo = 0;
            TotalAmmoLeft = _ammoAmount;
            ReloadWeapon();
        }

        public void EnableWeapon(LayerMask _aimLayer)
        {
            aimLayer = _aimLayer;
            weaponView.gameObject.SetActive(true);
        }
        public void DisableWeapon()
        {
            weaponView.gameObject.SetActive(false);
        }

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
                (Time.time > lastFireTime + weaponModel.WeaponFireRateInSeconds)
                ) ? true : false;
        }

        public bool IsEnabled() => weaponView.gameObject.activeSelf;
        public WeaponModel GetModel() => weaponModel;
        public WeaponView GetView() => weaponView;
    }
}