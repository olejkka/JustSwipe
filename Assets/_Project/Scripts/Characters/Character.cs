using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public event Action<Vector2Int> OnPositionChanged;
        public event Action<Character, int> OnHealthChanged;
        
        public Vector2Int Position { get; private set; }
        public CharacterConfig CharacterConfig { get; private set; }
        public Stats _stats;
        

        public Character(Vector2Int spawnPos, CharacterConfig characterConfig)
        {
            Position = spawnPos;
            CharacterConfig = characterConfig;
            
            _stats = CharacterConfig.BaseStats;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            OnPositionChanged?.Invoke(Position);
        }

        public void TakeDamage(int amount)
        {
            _stats.Health -= amount;
            OnHealthChanged?.Invoke(this, _stats.Health);
        }
    }
}