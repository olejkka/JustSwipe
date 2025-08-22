using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharactersStatsConfig",
        fileName = "CharactersStatsConfig"
    )]
    public class CharacterStatsConfig : ScriptableObject
    {
        public List<CharacterStatsEntry> CharacterStatsEntries = new();

        [Serializable]
        public class CharacterStatsEntry
        {
            public string Id;
            public Team team;
            public int BaseMaxHealth;
            public int BaseAttackDamage;
        }
        
        public string GetRandomCharacterIdByTeam(Team team)
        {
            var teamCharacters = CharacterStatsEntries
                .Where(entry => entry.team == team)
                .ToList();

            if (!teamCharacters.Any())
            {
                Debug.LogError($"Не найдены персонажи для команды: {team}");
                return null;
            }

            var randomIndex = Random.Range(0, teamCharacters.Count);
            return teamCharacters[randomIndex].Id;
        }
    }
}