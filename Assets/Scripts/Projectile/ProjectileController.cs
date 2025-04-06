using UnityEngine;

namespace Game.Projectile
{
    public abstract class ProjectileController
    {
        // Private Variables
        private ProjectileModel projectileModel;
        private ProjectileView projectileView;

        public ProjectileController(ProjectileData _projectileData, Transform _parentPanel,
            Vector3 _firePosition, Vector3 _fireDirection)
        {
            // Setting Variables
            projectileModel = new ProjectileModel(_projectileData);
            projectileView =
                Object.Instantiate(_projectileData.projectilePrefab, _parentPanel).GetComponent<ProjectileView>();
            projectileView.Init(this);

            // Setting Elements
            Reset(_projectileData, _firePosition, _fireDirection);
        }

        public void Reset(ProjectileData _projectileData, Vector3 _firePosition, Vector3 _fireDirection)
        {
            projectileModel.Reset(_projectileData);
            projectileView.ShowView();

            FireProjectile(_firePosition, _fireDirection);
        }

        private void FireProjectile(Vector3 _firePosition, Vector3 _fireDirection)
        {
            Rigidbody rigidbody = projectileView.GetRigidbody();

            // Making sure, projectile launches after a threshold from firepoint
            Vector3 newPosition = _firePosition + _fireDirection * 0.2f;
            Quaternion newRotation = Quaternion.LookRotation(_fireDirection);

            projectileView.transform.position = newPosition;
            projectileView.transform.rotation = newRotation;

            rigidbody.linearVelocity = _fireDirection * projectileModel.ProjectileSpeed;
            rigidbody.angularVelocity = Vector3.zero; // Resetting rotation momentum
        }

        // Getters
        public bool IsActive() => projectileView.gameObject.activeInHierarchy;
        public ProjectileModel GetModel() => projectileModel;
        public ProjectileView GetView() => projectileView;
    }
}