using ServiceLocator.Projectile;

namespace ServiceLocator.Weapon
{
    public class WeaponModel
    {
        public WeaponModel(WeaponData _weaponData)
        {
            WeaponType = _weaponData.weaponType;
            WeaponProjectileType = _weaponData.weaponProjectileType;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }
        public ProjectileType WeaponProjectileType { get; private set; }
    }
}