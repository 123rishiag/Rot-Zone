using Game.Enemy;
using Game.Spawn;
using Game.Weapon;
using System;
using UnityEngine;

namespace Game.Wave
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Scriptable Objects/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public WaveData[] waveData;
    }

    [Serializable]
    public class WaveData
    {
        public WaveType waveType;
        public PlayerSpawnData playerSpawnData;
        public EnemySpawnData[] enemySpawnDatas;
    }

    [Serializable]
    public class PlayerSpawnData : SpawnData
    {
        public int playerHealth = 5;
        public PlayerSpawnAmmoData[] playerSpawnAmmoDatas;
    }
    [Serializable]
    public class PlayerSpawnAmmoData
    {
        public WeaponType weaponType;
        public int ammoToAdd = 100;
    }
    [Serializable]
    public class EnemySpawnData : SpawnData
    {
        public EnemyType enemyType;
        public int enemyCount = 1;
    }
}