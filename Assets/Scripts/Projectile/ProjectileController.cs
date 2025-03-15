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

            // Making sure, projectile launches after a threshold from firepoint
            Vector3 newPosition = _firePoint.position + _firePoint.forward * 0.2f;
            projectileView.transform.position = newPosition;
            projectileView.transform.rotation = _firePoint.rotation;

            rigidbody.AddForce(_firePoint.forward * projectileModel.ProjectileSpeed, ForceMode.Impulse);
        }
    }
}