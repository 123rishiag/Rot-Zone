using Game.Enemy;
using System.Collections;
using UnityEngine;

namespace Game.Projectile
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
            projectileRigidBody = GetComponent<Rigidbody>();
            trailRenderer = GetComponent<TrailRenderer>();

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

        private void OnCollisionEnter(Collision _collision)
        {
            EnemyView enemyView = _collision.collider.GetComponentInParent<EnemyView>();
            if (enemyView != null)
            {
                Vector3 impactForce = projectileRigidBody.linearVelocity.normalized *
                                              projectileController.GetModel().ProjectileForce;
                int damage = projectileController.GetModel().ProjectileDamage;
                enemyView.HitImpactCoroutine(impactForce, damage, _collision);
                HideView();
            }
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