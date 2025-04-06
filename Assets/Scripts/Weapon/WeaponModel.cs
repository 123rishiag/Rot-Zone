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
            WeaponFireRateInSeconds = _weaponData.weaponFireRateInSeconds;

            WeaponAimLaserMaxDistance = _weaponData.weaponAimLaserMaxDistance;

            WeaponMaxCapacity = _weaponData.weaponMaxCapacity;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }

        public ProjectileType WeaponProjectileType { get; private set; }
        public WeaponFireType WeaponFireType { get; private set; }
        public float WeaponFireRateInSeconds { get; private set; }

        public float WeaponAimLaserMaxDistance { get; private set; }

        public int WeaponMaxCapacity { get; private set; }
    }
}