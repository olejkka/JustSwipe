using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Effects;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Character Effects",
        fileName = "Character Effects"
    )]
    public class CharacterEffectsConfig : ScriptableObject
    {
        public List<EffectDefinition> EffectEntries = new();
        
        
        public EffectDefinition GetEntryByDefinitionId(string definitionId) =>
            EffectEntries.FirstOrDefault(e => e.DefinitionId == definitionId);
        
        public EffectDefinition GetRandomEntry() =>
            EffectEntries.Count > 0 ? EffectEntries[Random.Range(0, EffectEntries.Count)] : null;
    }
}