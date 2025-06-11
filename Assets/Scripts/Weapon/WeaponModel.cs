using Game.Projectile;
using UnityEngine;

namespace Game.Weapon
{
    public class WeaponModel
    {
        public WeaponModel(WeaponData _weaponData)
        {
            WeaponType = _weaponData.weaponType;

            WeaponProjectileType = _weaponData.weaponProjectileType;
            WeaponFireType = _weaponData.weaponFireType;
            WeaponFireRateInSeconds = _weaponData.weaponFireRateInSeconds;
            WeaponKickBackFactor = _weaponData.weaponKickBackFactor;

            WeaponAimLaserMaxDistance = _weaponData.weaponAimLaserMaxDistance;
            WeaponGroundLayer = _weaponData.weaponGroundLayer;

            WeaponMaxCapacity = _weaponData.weaponMaxCapacity;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }

        public ProjectileType WeaponProjectileType { get; private set; }
        public WeaponFireType WeaponFireType { get; private set; }
        public float WeaponFireRateInSeconds { get; private set; }
        public int WeaponKickBackFactor { get; private set; }

        public float WeaponAimLaserMaxDistance { get; private set; }
        public LayerMask WeaponGroundLayer { get; private set; }

        public int WeaponMaxCapacity { get; private set; }
    }
}