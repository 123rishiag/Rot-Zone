using ServiceLocator.Player;
using ServiceLocator.Utility;
using System.Collections;
using UnityEngine;

namespace ServiceLocator.Enemy
{
    public class EnemyController : IStateOwner<EnemyController>
    {
        // Private Variables
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyAnimationController enemyAnimationController;

        public EnemyController Owner { get; set; }
        private EnemyStateMachine enemyStateMachine;

        // Private Services
        public PlayerService PlayerService;

        public EnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            PlayerService _playerService)
        {
            // Setting Variables
            enemyModel = new EnemyModel(_enemyData);
            enemyView = Object.Instantiate(_enemyData.enemyPrefab, _parentPanel).GetComponent<EnemyView>();
            enemyView.Init(this);
            enemyAnimationController = new EnemyAnimationController(enemyView.GetAnimator(), this);

            // Setting Services
            PlayerService = _playerService;

            // Setting Elements
            CreateStateMachine();
            Reset(_enemyData, _spawnPosition);
        }

        private void CreateStateMachine()
        {
            Owner = this;
            enemyStateMachine = new EnemyStateMachine(this);
        }

        public void Reset(EnemyData _enemyData, Vector3 _spawnPosition)
        {
            // Setting Variables
            enemyStateMachine.ChangeState(EnemyState.IDLE);

            enemyModel.Reset(_enemyData);
            enemyView.SetPosition(_spawnPosition);
            enemyView.ShowView();
        }

        public void Update()
        {
            enemyStateMachine.Update();
        }

        public void RotateTowardsPlayer()
        {
            Transform enemyTransform = Owner.GetTransform();
            Vector3 direction = (PlayerService.GetController().GetTransform().position -
                enemyTransform.position).normalized;

            direction.y = 0f;
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(
                    enemyTransform.rotation,
                    targetRotation,
                    Time.deltaTime * enemyModel.RotationSpeed
                );
        }

        public IEnumerator HitImpact(Vector3 _impactForce, Collision _hitCollision)
        {
            var hitPoint = _hitCollision.contacts[0].point;
            enemyStateMachine.ChangeState(EnemyState.DEAD);

            yield return new WaitForSeconds(0.1f);

            Rigidbody impactedRigidbody = _hitCollision.collider.attachedRigidbody;
            impactedRigidbody.AddForceAtPosition(_impactForce, hitPoint, ForceMode.Impulse);

            // Hide Enemy after 3 seconds of Death
            yield return new WaitForSeconds(3f);
            enemyView.HideView();
        }

        // Getters
        public bool IsPlayerDetected()
        {
            float detectionDistance = enemyModel.DetectionDistance;
            float detectionAngleDegree = enemyModel.DetectionAngleDegree;

            LayerMask layerMask = PlayerService.GetController().GetLayerMask();
            Transform target = PlayerService.GetController().GetTransform();

            Vector3 origin = enemyView.transform.position + Vector3.up;
            Vector3 toTarget = (target.position + Vector3.up - origin);
            float distance = toTarget.magnitude;

            // If Enemy is away from the distance
            if (distance > detectionDistance)
                return false;

            // Fetching Angle Between enemy and Player
            Vector3 forward = enemyView.transform.forward;
            toTarget.Normalize();
            float dot = Vector3.Dot(forward, toTarget);
            float angleToTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angleToTarget > detectionAngleDegree)
                return false;

            if (!Physics.Raycast(origin, toTarget, out RaycastHit hit, detectionDistance, layerMask.value))
                return false;

            if (hit.transform != target)
                return false;

            return true;
        }

        public bool IsActive() => enemyView.gameObject.activeInHierarchy;
        public EnemyModel GetModel() => enemyModel;
        public EnemyView GetView() => enemyView;
        public EnemyAnimationController GetAnimationController() => enemyAnimationController;
        public EnemyStateMachine GetEnemyStateMachine() => enemyStateMachine;
        public Transform GetTransform() => enemyView.transform;
    }
}