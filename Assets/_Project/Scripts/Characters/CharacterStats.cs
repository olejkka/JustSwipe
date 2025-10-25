using System;

namespace _Project.Scripts.Characters
{
    [Serializable]
    public struct CharacterStats
    {
        public int Health;
        public int Damage;

        public CharacterStats(int health, int damage)
        {
            Health = health;
            Damage = damage;
        }

        public CharacterStats Copy() => new CharacterStats(Health, Damage);
    }
}