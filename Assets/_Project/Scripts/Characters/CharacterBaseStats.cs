using System;

namespace _Project.Scripts.Characters
{
    [Serializable]
    public struct CharacterBaseStats
    {
        public int Health;
        public int Damage;
        public int AttackRange;
        public int AttackCD;

        public CharacterBaseStats(int health, int damage, int attackRange, int attackCD)
        {
            Health = health;
            Damage = damage;
            AttackRange = attackRange;
            AttackCD = attackCD;
        }

        public CharacterBaseStats Copy() => new CharacterBaseStats(Health, Damage, AttackRange, AttackCD);
    }
}
