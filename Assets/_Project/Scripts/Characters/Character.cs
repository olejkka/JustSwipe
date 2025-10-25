using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public Vector2Int Position { get; private set; }
        public Team Team { get; private set; }
        
        public int Health { get; private set; }
        public int Damage { get; private set; }
        
        public event Action<Vector2Int> OnPositionChanged;
        public event Action<int> OnHealthChanged;
        
        
        public Character(Vector2Int position, Team team, CharacterStats stats)
        {
            Position = position;
            Team = team;
            
            // Копируем статы из конфига в экземпляр персонажа
            Health = stats.Health;
            Damage = stats.Damage;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            OnPositionChanged?.Invoke(Position);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            OnHealthChanged?.Invoke(Health);
        }
    }
}