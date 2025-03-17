namespace ServiceLocator.Projectile
{
    public class ProjectileModel
    {
        public ProjectileModel(ProjectileData _projectileData)
        {
            Reset(_projectileData);
        }

        public void Reset(ProjectileData _projectileData)
        {
            ProjectileType = _projectileData.projectileType;
            ProjectileSpeed = _projectileData.projectileSpeed;
        }

        // Getters
        public ProjectileType ProjectileType { get; private set; }
        public float ProjectileSpeed { get; private set; }
    }
}