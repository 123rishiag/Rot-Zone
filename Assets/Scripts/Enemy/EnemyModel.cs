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
            MoveSpeed = _enemyData.moveSpeed;
        }

        public EnemyType EnemyType { get; private set; }
        public float MoveSpeed { get; private set; }
    }
}