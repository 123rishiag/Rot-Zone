using UnityEngine;

namespace ServiceLocator.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        // Private Variables
        private Rigidbody projectileRigidBody;
        private Renderer projectileRenderer;

        public void Init()
        {
            projectileRigidBody = GetComponentInChildren<Rigidbody>();
            projectileRenderer = GetComponentInChildren<Renderer>();
        }

        // Getters
        public Rigidbody GetRigidbody() => projectileRigidBody; 
        public Vector3 GetSize() => projectileRenderer.bounds.size;
    }
}