using Game.Projectile;
using System;
using UnityEngine;

namespace Game.Weapon
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
        [Range(0.1f, 2f)]
        public float weaponFireRateInSeconds = 1f;
        [Range(2, 9)]
        public int weaponKickBackFactor = 3;
        [Range(0.01f, 0.1f)]
        public float weaponSpreadFactor = 0.05f;

        [Header("Weapon Aim Settings")]
        public float weaponAimLaserMaxDistance = 5f;
        public float weaponAimSpeed = 100f;

        [Header("Weapon Ammo Settings")]
        public int weaponMaxCapacity = 30;
        [Range(1, 10)]
        public int weaponAmmoCountInSingleShot = 5; // Count of Ammo lost in single shot
        [Range(0f, 0.1f)]
        public float weaponAmmoGapInSingleShot = 0.1f; // Gap in Each fire in single shot
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
        // All WeaponIK Transforms for weapon alignments based on weapon type

        // Transform GameObject for Weapon Holder
        public Transform weaponHolder;

        // Right Hand Transform, how the character will hold weapon on right hand
        public Transform rightHand_TargetTransform;

        // How much Offset weapon should have after right hand is set for weapon to point forward
        public Vector3 rightHand_TargetOffset;

        // Left Hand Transforms after weapon is set on right hand
        public Transform leftHand_TargetTransform;

        // Left Hand Hint to change other left hand bones based on posture, so posture remains maintained
        public Transform leftHand_HintTransform;
    }
}