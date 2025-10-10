using Game.Enemy;
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
        }

        public void ShowView()
        {
            trailRenderer.Clear();
            gameObject.SetActive(true);
        }
        public void HideView()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision _collision)
        {
            if ((projectileController.GetModel().CollisionDamageLayerMask.value &
                (1 << _collision.collider.gameObject.layer)) != 0)
            {
                EnemyView enemyView = _collision.collider.GetComponentInParent<EnemyView>();
                if (enemyView != null)
                {
                    int damage = projectileController.GetModel().ProjectileDamage;
                    enemyView.HitImpact(damage);
                    projectileController.StartOnCollisionHideCoroutine();
                }
            }
            else if ((projectileController.GetModel().CollisionDestroyLayerMask.value &
                (1 << _collision.collider.gameObject.layer)) != 0)
            {
                projectileController.StartOnCollisionHideCoroutine();
            }
        }

        // Getters
        public Rigidbody GetRigidbody() => projectileRigidBody;
    }
}