using UnityEngine;

namespace _Project.Scripts.Characters.Movement
{
    public readonly struct ProjectileShot
    {
        public Character Attacker { get; }
        public Vector2Int Direction { get; }
        public Character Target { get; }
        public Vector2Int ImpactPosition { get; }


        public ProjectileShot(
            Character attacker,
            Vector2Int direction,
            Character target,
            Vector2Int impactPosition)
        {
            Attacker = attacker;
            Direction = direction;
            Target = target;
            ImpactPosition = impactPosition;
        }
    }
}
