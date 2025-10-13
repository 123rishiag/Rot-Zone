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

        private IEnumerator currentProjectileRoutine;
        private WaitForSeconds enableGravityOnDistanceYield;
        private WaitForSeconds hideProjectileOnTimeYield;
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
            enableGravityOnDistanceYield = new WaitForSeconds(timeInSeconds);
            hideProjectileOnTimeYield = new WaitForSeconds(5f); // Hiding Projectile in 5 seconds after gravity
            hideProjectileOnCollisionYield = new WaitForSeconds(0.01f);

            Reset(_projectileData, _firePosition, _fireDirection);
        }

        public void Reset(ProjectileData _projectileData, Vector3 _firePosition, Vector3 _fireDirection)
        {
            // Stopping existing projectile Coroutine if running
            if (currentProjectileRoutine != null)
            {
                miscService.GetController().StopManualCoroutine(currentProjectileRoutine);
            }

            projectileModel.Reset(_projectileData);
            projectileView.ShowView();
            FireProjectile(_firePosition, _fireDirection);
        }

        private void FireProjectile(Vector3 _firePosition, Vector3 _fireDirection)
        {
            Rigidbody rigidbody = projectileView.GetRigidbody();
            rigidbody.useGravity = false;

            // Making sure, projectile launches after a threshold from firepoint
            Vector3 newPosition = _firePosition + _fireDirection * 0.2f;
            Quaternion newRotation = Quaternion.LookRotation(_fireDirection);

            projectileView.transform.position = newPosition;
            projectileView.transform.rotation = newRotation;

            rigidbody.linearVelocity = _fireDirection * projectileModel.ProjectileSpeed;
            rigidbody.angularVelocity = Vector3.zero; // Resetting rotation momentum

            StartOnDistanceEnableGravityCoroutine();
        }
        private void StartOnDistanceEnableGravityCoroutine()
        {
            // Enabling Gravity After Distance Traveled
            currentProjectileRoutine = miscService.GetController().StartManualCoroutine(
                OnDistanceEnableGravityCoroutine(enableGravityOnDistanceYield));
        }
        private void StartOnTimeHideCoroutine()
        {
            // Hiding Projectile After Gravity Enabled
            currentProjectileRoutine =
                miscService.GetController().StartManualCoroutine(HideViewCoroutine(hideProjectileOnTimeYield));
        }
        public void StartOnCollisionHideCoroutine()
        {
            // Hiding Projectile After Collision
            currentProjectileRoutine =
                miscService.GetController().StartManualCoroutine(HideViewCoroutine(hideProjectileOnCollisionYield));
        }
        private IEnumerator OnDistanceEnableGravityCoroutine(WaitForSeconds _waitForSeconds)
        {
            yield return _waitForSeconds;
            Rigidbody rigidbody = projectileView.GetRigidbody();
            rigidbody.useGravity = true;
            StartOnTimeHideCoroutine();
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