using System;
using UnityEngine;

namespace ServiceLocator.Weapon
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Objects/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Weapon Settings")]
        public WeaponData[] weaponData;
    }

    [Serializable]
    public struct WeaponData
    {
        public WeaponType weaponType;
        public GameObject weaponPrefab;
    }

    [Serializable]
    public struct WeaponIKData
    {
        public WeaponType weaponType;
        public Transform weaponTypeHolder;
    }
}