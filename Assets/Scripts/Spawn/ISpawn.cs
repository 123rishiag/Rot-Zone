using System;
using UnityEngine;

namespace Game.Spawn
{
    public interface ISpawn<T> where T : SpawnData
    {
        public void OnSpawn(Func<Vector3> _spawnPositionFunc, T _spawnData);
    }
    public abstract class SpawnData { }
}