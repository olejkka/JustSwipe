using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        public string DefinitionId { get; private set; }
        public int InstanceId { get; private set; }
        public CharacterType CharacterType { get; private set; }
        public Team Team { get; private set; }
        public Vector2Int Position { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        private readonly List<CharacterEffect> _effects = new();
        public IReadOnlyList<CharacterEffect> Effects => _effects;
        public Character LastDamageSource { get; private set; }
        
        public event Action<Vector2Int, Vector2Int> OnPositionChanged;
        public event Action<int> OnHealthChanged;
        
        
        public Character(
            string definitionId,
            int instanceId,
            Vector2Int position,
            Team team, 
            CharacterBaseStats baseStats
            )
        {
            DefinitionId = definitionId;
            InstanceId = instanceId;
            Position = position;
            Team = team;
            Health = baseStats.Health;
            Damage = baseStats.Damage;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            
            OnPositionChanged?.Invoke(Position, vector);
        }

        public void TakeDamage(int amount, Character source = null)
        {
            Health -= amount;
            LastDamageSource = source;
            
            OnHealthChanged?.Invoke(Health);
        }
        
        public void AddEffect(CharacterEffect effect)
        {
            _effects.Add(effect);
            
            Debug.Log($"Add effect {effect} Type {effect.Type} Parameter {effect.Parameter} RemainingTurns {effect.RemainingTurns}");
        }

        public void TickEffects()
        {
            for (var i = _effects.Count - 1; i >= 0; i--)
            {
                var effect = _effects[i];
                effect.RemainingTurns--;

                Debug.Log($"Ticking effect {effect} Type {effect.Type} Parameter {effect.Parameter} RemainingTurns {effect.RemainingTurns}");
                
                if (effect.RemainingTurns <= 0)
                {
                    _effects.RemoveAt(i);
                    Debug.Log($"Effect {effect} Destroyed");
                }
                else
                    _effects[i] = effect;
            }
        }
    }
}