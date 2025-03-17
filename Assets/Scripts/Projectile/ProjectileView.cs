using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        // Private Variables
        private ProjectileController projectileController;
        private Rigidbody projectileRigidBody;

        public void Init(ProjectileController _projectileController)
        {
            // Setting Variables
            projectileController = _projectileController;

            projectileRigidBody = GetComponentInChildren<Rigidbody>();
        }

        // Setters
        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);
        // Getters
        public Rigidbody GetRigidbody() => projectileRigidBody;
    }
}