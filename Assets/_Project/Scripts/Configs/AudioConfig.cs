using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Audio",
        fileName = "Audio"
    )]
    public sealed class AudioConfig : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public SoundId Id;
            public AudioClip Clip;
            
            [Range(0f, 1f)]
            public float Volume;

            [Range(-3f, 3f)]
            public float Pitch;
        }

        [SerializeField] private Entry[] _entries;

        public Entry Get(SoundId id)
        {
            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i].Id == id)
                    return _entries[i];
            }

            return default;
        }
        
        public List<Entry> GetAll(SoundId id)
        {
            var result = new List<Entry>();

            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i].Id == id)
                    result.Add(_entries[i]);
            }

            return result;
        }
    }
    
    public enum SoundId
    {
        None = 0,
        Swipe = 1,
        CharacterDeath = 2,
        MenuMusic = 3,
        GameplayMusic = 4
    }
}