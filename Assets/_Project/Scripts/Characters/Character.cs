using System;
using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public Vector2Int Position { get; private set; }
        public CharacterType CharacterType { get; private set; }
        public Team Team { get; private set; }
        
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public Character LastDamageSource { get; private set; }
        
        public event Action<Vector2Int> OnPositionChanged;
        public event Action<int> OnHealthChanged;
        
        
        public Character(
            Vector2Int position,
            CharacterType characterType,
            Team team, 
            CharacterBaseStats baseStats
            )
        {
            Position = position;
            CharacterType = characterType;
            Team = team;
            Health = baseStats.Health;
            Damage = baseStats.Damage;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            
            OnPositionChanged?.Invoke(Position);
        }

        public void TakeDamage(int amount, Character source = null)
        {
            Health -= amount;
            LastDamageSource = source;
            
            OnHealthChanged?.Invoke(Health);
        }
    }
}