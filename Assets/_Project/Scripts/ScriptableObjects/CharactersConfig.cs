using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
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
            public Team Team;
            public Sprite Sprite;
            public CharacterStats BaseStats;
        }

        public CharacterEntry GetEntryByTeam(Team team)
        {
            return CharacterEntries.FirstOrDefault(e => e.Team == team);
        }
    }
}