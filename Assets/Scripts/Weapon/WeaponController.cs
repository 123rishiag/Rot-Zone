using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private PlayerController playerController;

    [Header("Weapon Prefabs")]
    [SerializeField] private WeaponData[] weaponData;

    public GameObject CreateWeapon(WeaponType _weaponType, Transform _parentTransform)
    {
        GameObject weaponPrefab = GetWeaponData(_weaponType).weaponPrefab;
        return Instantiate(weaponPrefab, _parentTransform);
    }

    public WeaponData GetWeaponData(WeaponType _weaponType) =>
        Array.Find(weaponData, w => w.weaponType == _weaponType);
}

public enum WeaponType
{
    NONE,
    PISTOL,
    RIFLE,
    SHOTGUN
}

[Serializable]
public struct WeaponData
{
    public WeaponType weaponType;
    public GameObject weaponPrefab;
}

[Serializable]
public struct WeaponTransform
{
    public WeaponType weaponType;
    public Transform weaponParentTransform;
}