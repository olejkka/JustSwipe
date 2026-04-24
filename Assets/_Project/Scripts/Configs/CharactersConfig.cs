using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<CharacterEntry> CharacterEntries = new();


        public CharacterEntry GetEntry(CharacterType type)
        {
            return CharacterEntries.FirstOrDefault(e => e.CharacterType == type);
        }
        
        public CharacterEntry GetRandomEntryByTeam(Team team)
        {
            var entries = CharacterEntries.Where(e => e.Team == team).ToList();
            return entries.Count > 0 ? entries[Random.Range(0, entries.Count)] : null;
        }
        
        public CharacterEntry GetRandomEntryByTeamExcept(Team team, CharacterType? excludedType)
        {
            var entries = CharacterEntries
                .Where(e => e.Team == team && (!excludedType.HasValue || e.CharacterType != excludedType.Value))
                .ToList();
            return entries.Count > 0 ? entries[Random.Range(0, entries.Count)] : null;
        }
    }
    
    [Serializable]
    public class CharacterEntry
    {
        public CharacterType CharacterType;
        public Team Team;
        public Sprite Icon;
        public CharacterAnimationData Animations;
        public CharacterBaseStats CharacterBaseStats;
        public int Price;
        public int Reward;
    }
    
    [Serializable]
    public class CharacterAnimationData
    {
        public float FrameRate = 8f;
        public Sprite[] Idle;
        public Sprite[] Move;
        public Sprite[] DealDamage;
        public Sprite[] TakeDamage;
        public Sprite[] Death;
    }
}