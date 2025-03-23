using UnityEngine;
using UnityEngine.AI;

namespace ServiceLocator.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        // Private Variables
        private CharacterController characterController;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        public void Init()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        public void SetPosition(Vector3 _spawnPosition)
        {
            transform.position = _spawnPosition;
        }
        public void ShowView()
        {
            gameObject.SetActive(true);
        }
        public void HideView()
        {
            gameObject.SetActive(false);
        }

        // Getters
        public CharacterController GetCharacterController() => characterController;
        public Animator GetAnimator() => animator;
        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;
    }
}
