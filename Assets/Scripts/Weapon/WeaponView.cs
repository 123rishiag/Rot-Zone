using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private LineRenderer aimLaser;

        // Private Variables
        private WeaponController weaponController;

        public void Init(WeaponController _weaponController)
        {
            // Setting Variables
            weaponController = _weaponController;
        }

        public void UpdateAimLaser()
        {
            Vector3 endPoint = firePoint.position + firePoint.forward *
                weaponController.GetModel().WeaponAimLaserMaxDistance;
            aimLaser.SetPosition(0, firePoint.position);
            aimLaser.SetPosition(1, endPoint);
        }

        // Getters
        public Transform GetFirePoint() => firePoint;
    }
}