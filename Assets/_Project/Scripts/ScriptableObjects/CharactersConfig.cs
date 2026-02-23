using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharactersConfig",
        fileName = "CharactersConfig"
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
        public Sprite[] Idle;
        public Sprite[] Move;
        public Sprite[] Attack;
        public Sprite[] Death;
        public float FrameRate = 8f;
    }
}