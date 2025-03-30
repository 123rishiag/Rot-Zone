using UnityEngine;

namespace ServiceLocator.Spawn
{
    public interface ISpawn
    {
        public void OnSpawn(Vector3 _spawnPosition);
    }
}