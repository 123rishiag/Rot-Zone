using ServiceLocator.Projectile;

namespace ServiceLocator.Weapon
{
    public class WeaponModel
    {
        public WeaponModel(WeaponData _weaponData)
        {
            WeaponType = _weaponData.weaponType;
            WeaponProjectileType = _weaponData.weaponProjectileType;
            WeaponAimLaserMaxDistance = _weaponData.weaponAimLaserMaxDistance;
            WeaponInitialAmmo = _weaponData.weaponInitialAmmo;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }
        public ProjectileType WeaponProjectileType { get; private set; }
        public float WeaponAimLaserMaxDistance { get; private set; }
        public int WeaponInitialAmmo { get; private set; }
    }
}