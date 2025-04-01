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

        public void UpdateAimLaser(Vector3 _aimTarget)
        {
            aimLaser.SetPosition(0, firePoint.position);
            aimLaser.SetPosition(1, _aimTarget);
        }


        // Getters
        public Transform GetFirePoint() => firePoint;
        public float GetAimDistance() => Vector3.Distance(aimLaser.GetPosition(0), aimLaser.GetPosition(1));
    }
}