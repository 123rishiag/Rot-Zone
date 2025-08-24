using Game.Misc;
using System.Collections;
using UnityEngine;

namespace Game.Projectile
{
    public abstract class ProjectileController
    {
        // Private Variables
        private ProjectileModel projectileModel;
        private ProjectileView projectileView;

        private Coroutine currentProjectileHideCoroutine;
        private WaitForSeconds hideProjectileOnDistanceYield;
        private WaitForSeconds hideProjectileOnCollisionYield;

        // Private Services
        private MiscService miscService;

        public ProjectileController(ProjectileData _projectileData, Transform _parentPanel,
            Vector3 _firePosition, Vector3 _fireDirection, int _fireDistance,
            MiscService _miscService)
        {
            // Setting Variables
            projectileModel = new ProjectileModel(_projectileData);
            projectileView =
                Object.Instantiate(_projectileData.projectilePrefab, _parentPanel).GetComponent<ProjectileView>();
            projectileView.Init(this);

            // Setting Services
            miscService = _miscService;

            // Setting Elements
            float timeInSeconds = (_fireDistance + 1f) / projectileModel.ProjectileSpeed; // Little Extra Threshold
            hideProjectileOnDistanceYield = new WaitForSeconds(timeInSeconds);
            hideProjectileOnCollisionYield = new WaitForSeconds(0.01f);

            Reset(_projectileData, _firePosition, _fireDirection);
        }

        public void Reset(ProjectileData _projectileData, Vector3 _firePosition, Vector3 _fireDirection)
        {
            // Stopping existing projectile Coroutine if running
            if (currentProjectileHideCoroutine != null)
            {
                miscService.GetController().StopManualCoroutine(currentProjectileHideCoroutine);
            }

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

            StartOnDistanceHideCoroutine();
        }
        private void StartOnDistanceHideCoroutine()
        {
            // Hiding Projectile After Distance Traveled
            currentProjectileHideCoroutine =
                miscService.GetController().StartManualCoroutine(HideViewCoroutine(hideProjectileOnDistanceYield));
        }
        public void StartOnCollisionHideCoroutine()
        {
            // Hiding Projectile After Collision
            currentProjectileHideCoroutine =
                miscService.GetController().StartManualCoroutine(HideViewCoroutine(hideProjectileOnCollisionYield));
        }
        private IEnumerator HideViewCoroutine(WaitForSeconds _waitForSeconds)
        {
            yield return _waitForSeconds;
            projectileView.HideView();
        }

        // Getters
        public bool IsActive() => projectileView.gameObject.activeInHierarchy;
        public ProjectileModel GetModel() => projectileModel;
        public ProjectileView GetView() => projectileView;
    }
}