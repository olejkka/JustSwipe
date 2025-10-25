using System;

namespace _Project.Scripts.Characters.Structs
{
    [Serializable]
    public struct CharacterBaseStats
    {
        public int Health;
        public int Damage;

        public CharacterBaseStats(int health, int damage)
        {
            Health = health;
            Damage = damage;
        }

        public CharacterBaseStats Copy() => new CharacterBaseStats(Health, Damage);
    }
}