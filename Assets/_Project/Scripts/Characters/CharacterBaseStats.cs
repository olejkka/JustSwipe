using System;

namespace _Project.Scripts.Characters
{
    [Serializable]
    public struct CharacterBaseStats
    {
        public int Health;
        public int Damage;
        public int AttackRange;

        public CharacterBaseStats(int health, int damage, int attackRange)
        {
            Health = health;
            Damage = damage;
            AttackRange = attackRange;
        }

        public CharacterBaseStats Copy() => new CharacterBaseStats(Health, Damage, AttackRange);
    }
}
