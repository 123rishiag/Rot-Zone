namespace Game.Enemy
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

            MaxHealth = _enemyData.maxHealth;

            IdleDuration = _enemyData.idleDuration;
            PatrolSpeed = _enemyData.patrolSpeed;
            PatrolMaxRadius = _enemyData.patrolMaxRadius;
            RotationSpeed = _enemyData.rotationSpeed;
            ChaseSpeed = _enemyData.chaseSpeed;

            AttackForce = _enemyData.attackForce;
            AttackDamage = _enemyData.attackDamage;

            DetectionMinDistance = _enemyData.detectionMinDistance;
            DetectionMaxDistance = _enemyData.detectionMaxDistance;
            DetectionAngleDegree = _enemyData.detectionAngleDegree;
            DetectionMinScreamDistance = _enemyData.detectionMinScreamDistance;
            DetectionIncreaseFactor = _enemyData.detectionIncreaseFactor;
            StopDistance = _enemyData.stopDistance;
        }

        public EnemyType EnemyType { get; private set; }

        public int MaxHealth { get; private set; }

        public float IdleDuration { get; private set; }
        public float PatrolSpeed { get; private set; }
        public float PatrolMaxRadius { get; private set; }
        public float RotationSpeed { get; private set; }
        public float ChaseSpeed { get; private set; }

        public float AttackForce { get; private set; }
        public int AttackDamage { get; private set; }

        public float DetectionMinDistance { get; private set; }
        public float DetectionMaxDistance { get; private set; }
        public float DetectionAngleDegree { get; private set; }
        public float DetectionMinScreamDistance { get; private set; }
        public float DetectionIncreaseFactor { get; private set; }
        public float StopDistance { get; private set; }
    }
}