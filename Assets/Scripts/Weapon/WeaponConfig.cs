using ServiceLocator.Projectile;
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
    public class WeaponData
    {
        public WeaponType weaponType;
        public WeaponView weaponPrefab;
        public ProjectileType weaponProjectileType;
        public float weaponAimLaserMaxDistance = 5f;
    }

    [Serializable]
    public struct WeaponIKData
    {
        public WeaponType weaponType;
        public WeaponTransform weaponTransform;
    }

    [Serializable]
    public struct WeaponTransform
    {
        public Transform weaponHolder;
        public Transform rightHand_TargetTransform;
        public Transform leftHand_TargetTransform;
        public Transform leftHand_HintTransform;
    }
}