using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class Character
    {
        // identity
        public string DefinitionId { get; private set; }
        public int InstanceId { get; private set; }
        public CharacterType CharacterType { get; private set; }
        public Team Team { get; private set; }
        
        // state
        public Vector2Int Position { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int BonusHealth { get; private set; }
        public int BonusDamage { get; private set; }
        public int TotalDamage => Damage + BonusDamage;
        public Character LastDamageSource { get; private set; }

        // effects
        private readonly List<CharacterEffect> _effects = new();
        public IReadOnlyList<CharacterEffect> Effects => _effects;
        
        // events
        public event Action<Vector2Int, Vector2Int> OnPositionChanged;
        public event Action OnStatsChanged;
        public event Action<int> OnDamageTaken; 
        
        
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
            if (amount <= 0)
                return;
            
            LastDamageSource = source;
            
            var remaining = AbsorbBonusHealth(amount);

            if (remaining > 0) 
                Health -= remaining;

            OnDamageTaken?.Invoke(amount);
            OnStatsChanged?.Invoke();
        }

        private int AbsorbBonusHealth(int amount)
        {
            var remaining = amount;
            
            for (var i = 0; i < _effects.Count && remaining > 0;)
            {
                var effect = _effects[i];
                
                if (effect.Type != EffectType.HealthIncrease || effect.Parameter <= 0)
                {
                    i++;
                    continue;
                }

                var absorbed = Math.Min(remaining, effect.Parameter);
                effect.Parameter -= absorbed;
                BonusHealth -= absorbed;
                remaining -= absorbed;

                
                if (effect.Parameter <= 0)
                {
                    _effects.RemoveAt(i);
                }
                else
                {
                    _effects[i] = effect;
                    i++;
                }
            }
            
            return remaining;
        }
        
        public void AddBonusHealth(int amount)
        {
            BonusHealth += amount;
            
            OnStatsChanged?.Invoke();
        }

        public void AddBonusDamage(int amount)
        {
            BonusDamage += amount;
            
            OnStatsChanged?.Invoke();
        }

        public void AddEffect(CharacterEffect effect) => 
            _effects.Add(effect);

        public void TickEffects()
        {
            for (var i = _effects.Count - 1; i >= 0; i--)
            {
                var effect = _effects[i];
                effect.RemainingTurns--;
                
                if (effect.RemainingTurns <= 0)
                {
                    RemoveBonusFromEffect(effect);
                    _effects.RemoveAt(i);
                }
                else
                    _effects[i] = effect;
            }
        }
        
        private void RemoveBonusFromEffect(CharacterEffect effect)
        {
            switch (effect.Type)
            {
                case EffectType.HealthIncrease:
                    BonusHealth -= effect.Parameter;
                    break;
                // case EffectType.DamageIncrease: // когда добавите тип
                //     BonusDamage -= effect.Parameter;
                    break;
            }
            
            OnStatsChanged?.Invoke();
        }
    }
}