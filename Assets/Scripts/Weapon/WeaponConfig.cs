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
        [Header("Weapon Details")]
        public WeaponType weaponType;
        public WeaponView weaponPrefab;

        [Header("Weapon Fire Settings")]
        public ProjectileType weaponProjectileType;
        public WeaponFireType weaponFireType;
        public float weaponFireRateInSeconds = 1f;

        [Header("Weapon Aim Settings")]
        public float weaponAimLaserMaxDistance = 5f;

        [Header("Weapon Ammo Settings")]
        public int weaponMaxCapacity = 30;
    }

    [Serializable]
    public struct WeaponIKData
    {
        public WeaponType weaponType;
        public WeaponTransform weaponTransform;
    }

    [Serializable]
    public class WeaponTransform
    {
        public Transform weaponHolder;
        public Transform rightHand_TargetTransform;
        public Transform leftHand_TargetTransform;
        public Transform leftHand_HintTransform;
        public float weaponVerticalOffeset = 0f;
    }
}