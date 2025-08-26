using UnityEngine;
using UnityEngine.UI;

namespace Game.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private LineRenderer aimLaser;
        [SerializeField] private Image crossHairUI;

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

        public void UpdateCrossHairUIColor(bool _isTargetInHit)
        {
            crossHairUI.color = _isTargetInHit ? Color.red : Color.green;
        }

        // Getters
        public Transform GetFirePoint() => firePoint;
    }
}