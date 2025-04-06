using Game.Event;
using Game.Player;
using Game.Utility;
using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public abstract class EnemyController : IStateOwner<EnemyController>
    {
        // Private Variables
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private EnemyAnimationController enemyAnimationController;

        public EnemyController Owner { get; set; }
        private EnemyStateMachine enemyStateMachine;

        public float DetectionDistance { get; set; }
        private int currentHealth;

        // Private Services
        public EventService EventService { get; private set; }
        private PlayerService playerService;

        public EnemyController(EnemyData _enemyData, Transform _parentPanel, Vector3 _spawnPosition,
            EventService _eventService, PlayerService _playerService)
        {
            // Setting Variables
            enemyModel = new EnemyModel(_enemyData);
            enemyView = Object.Instantiate(_enemyData.enemyPrefab, _parentPanel).GetComponent<EnemyView>();
            enemyView.Init(this);
            enemyAnimationController = new EnemyAnimationController(enemyView.GetAnimator(), this);

            // Setting Services
            EventService = _eventService;
            playerService = _playerService;

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

            DetectionDistance = enemyModel.DetectionMaxDistance;
            currentHealth = enemyModel.MaxHealth;

            enemyView.SetPosition(_spawnPosition);
            enemyView.SetRagDollActive(true);
            enemyView.SetTrailRenderActive(false);
            enemyView.ShowView();
        }

        public void Update()
        {
            enemyStateMachine.Update();
            enemyView.DrawDetectionCone();
        }

        public void RotateTowardsPlayer()
        {
            Transform enemyTransform = Owner.GetTransform();
            Vector3 direction = (playerService.GetController().GetTransform().position -
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

        public IEnumerator HitImpact(Vector3 _impactForce, int _damage, Collision _hitCollision)
        {
            enemyStateMachine.ChangeState(EnemyState.IDLE);
            DecreaseHealth(_damage);
            var hitPoint = _hitCollision.contacts[0].point;

            yield return new WaitForEndOfFrame();

            if (currentHealth != 0)
            {
                enemyStateMachine.ChangeState(EnemyState.HURT);
            }
            else
            {
                enemyStateMachine.ChangeState(EnemyState.DEAD);
                yield return new WaitForEndOfFrame();

                Rigidbody impactedRigidbody = _hitCollision.collider.attachedRigidbody;
                if (impactedRigidbody != null)
                {
                    impactedRigidbody.AddForceAtPosition(_impactForce, hitPoint, ForceMode.Impulse);
                }
            }
        }
        private void DecreaseHealth(int _damage)
        {
            currentHealth -= _damage;
            if (currentHealth < 0)
                currentHealth = 0;
        }

        // Getters
        public bool IsPlayerDetected()
        {
            float detectionAngleDegree = enemyModel.DetectionAngleDegree;
            float detectionAngleHalf = detectionAngleDegree / 2f;

            LayerMask layerMask = playerService.GetController().GetLayerMask();
            Transform target = playerService.GetController().GetTransform();

            Vector3 origin = enemyView.transform.position + Vector3.up;
            Vector3 toTarget = (target.position + Vector3.up - origin);
            float distance = toTarget.magnitude;

            if (distance > DetectionDistance)
                return false;

            // Normalizing direction and computing angle using dot product
            Vector3 forward = enemyView.transform.forward;
            toTarget.Normalize();
            float dot = Vector3.Dot(forward, toTarget);
            float angleToTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

            // Returning if player is outside detection angle
            if (angleToTarget > detectionAngleHalf)
                return false;

            if (!Physics.Raycast(origin, toTarget, out RaycastHit hit, DetectionDistance, layerMask.value))
                return false;

            return hit.transform == target;
        }
        public float GetDistanceFromPlayer()
        {
            float distance = Vector3.Distance(enemyView.transform.position, GetPlayerPosition());
            return distance;
        }
        public Vector3 GetPlayerPosition() => playerService.GetController().GetTransform().position;
        public bool IsActive() => enemyView.gameObject.activeInHierarchy;
        public EnemyModel GetModel() => enemyModel;
        public EnemyView GetView() => enemyView;
        public EnemyAnimationController GetAnimationController() => enemyAnimationController;
        public EnemyStateMachine GetEnemyStateMachine() => enemyStateMachine;
        public Transform GetTransform() => enemyView.transform;
    }
}