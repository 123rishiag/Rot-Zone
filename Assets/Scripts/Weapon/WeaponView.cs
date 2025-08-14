using UnityEngine;

namespace Game.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private LineRenderer aimLaser;
        [SerializeField] private Transform aimCrosshair;

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
        public void UpdateAimCrosshair(Vector3 _position, Vector3 _direction)
        {
            aimCrosshair.position = _position;
            aimCrosshair.forward = _direction;
        }

        // Getters
        public Transform GetFirePoint() => firePoint;
    }
}