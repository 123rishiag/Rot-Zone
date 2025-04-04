using System;
using UnityEngine;

namespace ServiceLocator.Sound
{
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Scriptable Objects/SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        public SoundData[] soundData;
    }

    [Serializable]
    public class SoundData
    {
        public SoundType soundType;
        public AudioClip soundClip;
    }
}