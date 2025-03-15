using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        // Private Variables
        private Rigidbody projectileRigidBody;

        public void Init()
        {
            projectileRigidBody = GetComponentInChildren<Rigidbody>();
        }

        // Getters
        public Rigidbody GetRigidbody() => projectileRigidBody; 
    }
}