using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
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
        
        public CharacterDefinition GetRandomEntryByTeamExcept(Team team, CharacterType? excludedType)
        {
            var entries = CharacterEntries
                .Where(e => e.Team == team && (!excludedType.HasValue || e.CharacterType != excludedType.Value))
                .ToList();
            return entries.Count > 0 ? entries[Random.Range(0, entries.Count)] : null;
        }
    }
}