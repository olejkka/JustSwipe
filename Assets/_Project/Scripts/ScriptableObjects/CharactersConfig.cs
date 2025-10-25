using System;
using System.Collections.Generic;
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
        public List<CharacterEntry> Entries = new();

        [Serializable]
        public class CharacterEntry
        {
            [Header("Identity")]
            public string Id;
            public Team Team;
            
            [Header("Visuals")]
            public Sprite Sprite;
            
            [Header("Stats")]
            public int MaxHealth = 100;
            public int AttackDamage = 10;
        }

        public CharacterEntry GetEntryByTeam(Team team)
        {
            return Entries.Find(e => e.Team == team);
        }
        
        public CharacterEntry GetEntryById(string id)
        {
            return Entries.Find(e => e.Id == id);
        }
    }
}