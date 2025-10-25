using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharactersConfig",
        fileName = "CharactersConfig"
    )]
    public class CharactersConfig : ScriptableObject
    {
        public List<CharacterEntry> CharacterEntries = new();

        [Serializable]
        public class CharacterEntry
        {
            public CharacterType CharacterType;
            public Team Team;
            public Sprite Sprite;
            public CharacterBaseStats CharacterBaseStats;
        }

        public CharacterEntry GetEntry(CharacterType type)
        {
            return CharacterEntries.FirstOrDefault(e => e.CharacterType == type);
        }
    }
}