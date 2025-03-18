using System.Collections;
using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        // Private Variables
        private ProjectileController projectileController;
        private Rigidbody projectileRigidBody;
        private TrailRenderer trailRenderer;

        public void Init(ProjectileController _projectileController)
        {
            // Setting Variables
            projectileController = _projectileController;
            projectileRigidBody = GetComponentInChildren<Rigidbody>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();

            StartCoroutine(HideViewCoroutine(projectileController.GetModel().ProjectileNoActivityDisableTime));
        }

        public void ShowView()
        {
            trailRenderer.Clear();
            gameObject.SetActive(true);
            StartCoroutine(HideViewCoroutine(projectileController.GetModel().ProjectileNoActivityDisableTime));
        }
        public void HideView()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        // Getters
        public Rigidbody GetRigidbody() => projectileRigidBody;

        private IEnumerator HideViewCoroutine(float _seconds)
        {
            yield return new WaitForSeconds(_seconds);
            HideView();
        }

    }
}