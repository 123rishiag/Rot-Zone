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
        GameObject weapon = Instantiate(weaponPrefab, _parentTransform);
        Transform rightHand_TargetTransform = weapon.transform.Find("RightHand_Target");

        weapon.transform.position = rightHand_TargetTransform.position;
        weapon.transform.rotation = rightHand_TargetTransform.rotation;
        weapon.transform.localScale = rightHand_TargetTransform.localScale;
        return weapon;
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