using UnityEngine;

namespace ServiceLocator.Projectile
{
    public abstract class ProjectileController
    {
        // Private Variables
        private ProjectileModel projectileModel;
        private ProjectileView projectileView;

        public ProjectileController(ProjectileData _projectileData, Transform _parentPanel, Transform _firePoint)
        {
            // Setting Variables
            projectileModel = new ProjectileModel(_projectileData);
            projectileView =
                Object.Instantiate(_projectileData.projectilePrefab, _parentPanel).GetComponent<ProjectileView>();
            projectileView.Init(this);

            // Setting Elements
            Reset(_projectileData, _firePoint);
        }

        public void Reset(ProjectileData _projectileData, Transform _firePoint)
        {
            projectileModel.Reset(_projectileData);
            projectileView.ShowView();

            FireProjectile(_firePoint);
        }

        private void FireProjectile(Transform _firePoint)
        {
            Rigidbody rigidbody = projectileView.GetRigidbody();

            // Making sure, projectile launches after a threshold from firepoint
            Vector3 newPosition = _firePoint.position + _firePoint.forward * 0.2f;
            projectileView.transform.position = newPosition;
            projectileView.transform.rotation = _firePoint.rotation;

            rigidbody.linearVelocity = Vector3.zero; // Resetting velocity
            rigidbody.angularVelocity = Vector3.zero; // Resetting rotation momentum
            rigidbody.linearVelocity = _firePoint.forward * projectileModel.ProjectileSpeed;
        }

        // Getters
        public bool IsActive() => projectileView.gameObject.activeInHierarchy;
        public ProjectileModel GetModel() => projectileModel;
        public ProjectileView GetView() => projectileView;
    }
}