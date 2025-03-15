using UnityEngine;

namespace ServiceLocator.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private Transform firePoint;

        // Getters
        public Transform GetFirePoint() => firePoint;
    }
}