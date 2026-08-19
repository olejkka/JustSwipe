using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Characters",
        fileName = "Characters"
    )]
    public class CharactersConfig : ScriptableObject
    {
        public List<CharacterDefinition> CharacterEntries = new();


        public CharacterDefinition GetEntryByDefinitionId(string definitionId) => 
            CharacterEntries.FirstOrDefault(e => e.DefinitionId == definitionId);

        public CharacterDefinition GetRandomEntryByTeam(Team team)
        {
            var entries = CharacterEntries.Where(e => e.Team == team).ToList();
            return entries.Count > 0 ? entries[Random.Range(0, entries.Count)] : null;
        }
        
        public CharacterDefinition GetRandomEntryByTeamExcept(Team team, string excludedDefinitionId)
        {
            var entries = CharacterEntries
                .Where(e => e.Team == team && (string.IsNullOrEmpty(excludedDefinitionId) || e.DefinitionId != excludedDefinitionId))
                .ToList();
            return entries.Count > 0 ? entries[Random.Range(0, entries.Count)] : null;
        }
    }
}