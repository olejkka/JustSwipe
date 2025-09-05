using System;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public event Action<Vector2Int> OnPositionChanged;
        public event Action<Character, int> OnHealthChanged;
        
        public Vector2Int Position { get; private set; }
        public CharacterConfig CharacterConfig { get; private set; }
        public Stats Stats;
        
        
        public Character(Vector2Int spawnPos, CharacterConfig characterConfig)
        {
            Position = spawnPos;
            CharacterConfig = characterConfig;
            
            Stats = CharacterConfig.BaseStats;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            OnPositionChanged?.Invoke(Position);
        }

        public void TakeDamage(int amount)
        {
            Stats.Health -= amount;
            OnHealthChanged?.Invoke(this, Stats.Health);
        }
    }
}