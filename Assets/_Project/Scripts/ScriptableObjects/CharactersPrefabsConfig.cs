using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharactersPrefabsConfig",
        fileName = "CharactersPrefabsConfig"
    )]
    public class CharactersPrefabsConfig : ScriptableObject
    {
        public List<CharacterPrefabEntry> Characters = new();
        
        [Serializable]
        public class CharacterPrefabEntry
        {
            public string Id;
            public GameObject Prefab;
        }
    }
}