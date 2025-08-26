using Game.Projectile;

namespace Game.Weapon
{
    public class WeaponModel
    {
        public WeaponModel(WeaponData _weaponData)
        {
            WeaponType = _weaponData.weaponType;

            WeaponProjectileType = _weaponData.weaponProjectileType;
            WeaponFireType = _weaponData.weaponFireType;
            WeaponRangeDistanceInMeters = _weaponData.weaponRangeDistanceInMeters;
            WeaponFireRateInSeconds = _weaponData.weaponFireRateInSeconds;
            WeaponKickBackFactor = _weaponData.weaponKickBackFactor;
            WeaponSpreadFactor = _weaponData.weaponSpreadFactor;

            WeaponAimSpeed = _weaponData.weaponAimSpeed;

            WeaponMaxCapacity = _weaponData.weaponMaxCapacity;
            WeaponAmmoCountInSingleShot = _weaponData.weaponAmmoCountInSingleShot;
            WeaponAmmoGapInSingleShot = _weaponData.weaponAmmoGapInSingleShot;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }

        public ProjectileType WeaponProjectileType { get; private set; }
        public WeaponFireType WeaponFireType { get; private set; }
        public int WeaponRangeDistanceInMeters { get; private set; }
        public float WeaponFireRateInSeconds { get; private set; }
        public int WeaponKickBackFactor { get; private set; }
        public float WeaponSpreadFactor { get; private set; }

        public float WeaponAimSpeed { get; private set; }

        public int WeaponMaxCapacity { get; private set; }
        public int WeaponAmmoCountInSingleShot { get; private set; }
        public float WeaponAmmoGapInSingleShot { get; private set; }
    }
}