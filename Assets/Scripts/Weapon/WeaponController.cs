using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponController
    {
        // Private Variables
        private WeaponModel weaponModel;
        private WeaponView weaponView;

        public WeaponController(WeaponView _weaponPrefab, Transform _parentPanel)
        {
            weaponModel = new WeaponModel();
            weaponView = Object.Instantiate(_weaponPrefab, _parentPanel).GetComponent<WeaponView>();
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
    }
}