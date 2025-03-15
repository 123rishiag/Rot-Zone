using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileController
    {
        private const float fixedProjectileSpeed = 20f;

        // Private Variables
        private ProjectileModel projectileModel;
        private ProjectileView projectileView;

        public ProjectileController(ProjectileData _projectileData, Transform _parentPanel, Transform _firePoint)
        {
            // Setting Variables
            projectileModel = new ProjectileModel(_projectileData);
            projectileView =
                Object.Instantiate(_projectileData.projectilePrefab, _parentPanel).GetComponent<ProjectileView>();
            projectileView.Init();

            // Setting Elements
            FireProjectile(_firePoint);
        }

        private void FireProjectile(Transform _firePoint)
        {
            Rigidbody rigidbody = projectileView.GetRigidbody();
            projectileView.transform.position = new Vector3(
                _firePoint.position.x, 
                _firePoint.position.y,
                _firePoint.position.z - projectileView.GetSize().z + 0.2f);
            rigidbody.AddForce(_firePoint.forward * projectileModel.ProjectileSpeed, ForceMode.Impulse);
        }
    }
}