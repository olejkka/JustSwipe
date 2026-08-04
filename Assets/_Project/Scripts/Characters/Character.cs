using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Effects;
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

        // base stats caps
        public int MaxHealth { get; }
        public int MaxDamage { get; }

        // state
        public Vector2Int Position { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int BonusHealth { get; private set; }
        public int BonusDamage { get; private set; }
        public int TotalDamage => Damage + BonusDamage;
        public Character LastDamageSource { get; private set; }

        // effects
        private readonly List<Effect> _effects = new();
        public IReadOnlyList<Effect> Effects => _effects;

        // events
        public event Action<Vector2Int, Vector2Int> OnPositionChanged;
        public event Action OnStatsChanged;
        public event Action<int> OnDamageTaken;

        
        public Character(
            string definitionId,
            int instanceId,
            Vector2Int position,
            Team team,
            CharacterBaseStats baseStats)
        {
            DefinitionId = definitionId;
            InstanceId = instanceId;
            Position = position;
            Team = team;

            MaxHealth = baseStats.Health;
            MaxDamage = baseStats.Damage;

            Health = MaxHealth;
            Damage = MaxDamage;
        }

        public void Move(Vector2Int vector)
        {
            Position += vector;
            OnPositionChanged?.Invoke(Position, vector);
        }
        
        public void ChangeHealth(int delta, Character source = null)
        {
            if (delta == 0)
                return;

            if (delta < 0)
            {
                LastDamageSource = source;
                var damageAmount = -delta;

                var remaining = AbsorbBonusHealth(damageAmount);
                
                if (remaining > 0)
                    Health = ClampToBase(Health - remaining, MaxHealth);

                OnDamageTaken?.Invoke(damageAmount);
                OnStatsChanged?.Invoke();
                return;
            }
            
            Health = ClampToBase(Health + delta, MaxHealth);

            if (HasActiveBonusHealthEffect())
                BonusHealth = ClampToBase(BonusHealth + delta, MaxHealth);

            OnStatsChanged?.Invoke();
        }
        
        public void ChangeDamage(int delta, bool affectBaseDamage = false)
        {
            if (delta == 0)
                return;

            if (affectBaseDamage)
                Damage = ClampToBase(Damage + delta, MaxDamage);
            else
                BonusDamage = ClampToBase(BonusDamage + delta, MaxDamage);

            OnStatsChanged?.Invoke();
        }

        public void AddEffect(Effect effect) => _effects.Add(effect);

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
                {
                    _effects[i] = effect;
                }
            }
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
                BonusHealth = ClampToBase(BonusHealth - absorbed, MaxHealth);
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

        private bool HasActiveBonusHealthEffect()
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Type == EffectType.HealthIncrease && _effects[i].RemainingTurns > 0)
                    return true;
            }

            return false;
        }

        private void RemoveBonusFromEffect(Effect effect)
        {
            switch (effect.Type)
            {
                case EffectType.HealthIncrease:
                    BonusHealth = ClampToBase(BonusHealth - effect.Parameter, MaxHealth);
                    break;
                case EffectType.DamageIncrease:
                    BonusDamage = ClampToBase(BonusDamage - effect.Parameter, MaxDamage);
                    break;
                case EffectType.DamageDecrease:
                    ChangeDamage(effect.Parameter, affectBaseDamage: true);
                    break;
            }

            OnStatsChanged?.Invoke();
        }

        public void AddBonusHealth(int amount)
        {
            BonusHealth = ClampToBase(BonusHealth + amount, MaxHealth);
            OnStatsChanged?.Invoke();
        }

        public void AddBonusDamage(int amount)
        {
            BonusDamage = ClampToBase(BonusDamage + amount, MaxDamage);
            OnStatsChanged?.Invoke();
        }

        private static int ClampToBase(int value, int max) =>
            Math.Clamp(value, 0, max);
    }
}