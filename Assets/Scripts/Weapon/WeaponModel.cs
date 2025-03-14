namespace ServiceLocator.Weapon
{
    public class WeaponModel
    {
        public WeaponModel(WeaponData _weaponData)
        {
            WeaponType = _weaponData.weaponType;
        }

        // Getters
        public WeaponType WeaponType { get; private set; }
    }
}