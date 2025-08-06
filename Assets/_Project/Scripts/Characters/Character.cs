using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public event Action<Vector2Int> OnPositionChanged;
        public Team Team { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public Vector2Int Position { get; private set; }
        
        
        public Character(Team team, int health, int damage, Vector2Int position)
        {
            Team = team;
            Health = health;
            Damage = damage;
            Position = position;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            OnPositionChanged?.Invoke(Position);
        }
    }
}