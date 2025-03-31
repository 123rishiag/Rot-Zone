using System;
using UnityEngine;

namespace ServiceLocator.Spawn
{
    public interface ISpawn<T> where T : SpawnData
    {
        public void OnSpawn(Func<Vector3> _spawnPositionFunc, T _spawnData);
    }
    public abstract class SpawnData { }
}