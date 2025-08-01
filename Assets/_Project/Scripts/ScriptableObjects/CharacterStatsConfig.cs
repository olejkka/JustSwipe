using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharactersStatsConfig",
        fileName = "CharactersStatsConfig"
    )]
    public class CharacterStatsConfig : ScriptableObject
    {
        public List<CharacterStatsEntry> Characters = new();
        
        [Serializable]
        public class CharacterStatsEntry
        {
            public string Id;
            public Team team;
            public int BaseMaxHealth;
            public int BaseAttackDamage;
        }
    }
}