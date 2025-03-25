namespace ServiceLocator.Enemy
{
    public class EnemyModel
    {
        public EnemyModel(EnemyData _enemyData)
        {
            Reset(_enemyData);
        }

        public void Reset(EnemyData _enemyData)
        {
            EnemyType = _enemyData.enemyType;
            IdleDuration = _enemyData.idleDuration;
            PatrolSpeed = _enemyData.patrolSpeed;
            PatrolMaxRadius = _enemyData.patrolMaxRadius;
            PatrolStopDistance = _enemyData.patrolStopDistance;
            RotationSpeed = _enemyData.rotationSpeed;
            DetectionDistance = _enemyData.detectionDistance;
            DetectionAngleDegree = _enemyData.detectionAngleDegree;
            ChaseSpeed = _enemyData.chaseSpeed;
            StopDistance = _enemyData.stopDistance;
            IsGizmosEnabled = _enemyData.isGizmosEnabled;
        }

        public EnemyType EnemyType { get; private set; }
        public float IdleDuration { get; private set; }
        public float PatrolSpeed { get; private set; }
        public float PatrolMaxRadius { get; private set; }
        public float PatrolStopDistance { get; private set; }
        public float RotationSpeed { get; private set; }
        public float DetectionDistance { get; private set; }
        public float DetectionAngleDegree { get; private set; }
        public float ChaseSpeed { get; private set; }
        public float StopDistance { get; private set; }
        public bool IsGizmosEnabled { get; private set; }
    }
}