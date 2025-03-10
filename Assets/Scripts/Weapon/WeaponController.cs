using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private PlayerController playerController;

    [Header("Weapon Prefabs")]
    [SerializeField] private WeaponData[] weaponData;

    public GameObject CreateWeapon(WeaponType _weaponType)
    {
        GameObject weaponPrefab = GetWeaponData(_weaponType).weaponPrefab;
        GameObject weapon = Instantiate(weaponPrefab);
        return weapon;
    }

    private WeaponData GetWeaponData(WeaponType _weaponType) =>
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
public struct WeaponIKData
{
    public WeaponType weaponType;
    public Transform weaponTypeHolder;
}