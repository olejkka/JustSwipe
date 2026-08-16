using System.Collections.Generic;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersMeleeResolver
    {
        public void Resolve(IReadOnlyList<MeleeCollision> collisions)
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                var collision = collisions[i];

                collision.Attacker.PerformMeleeAttack();
                collision.Defender.ChangeHealth(-collision.Attacker.TotalDamage, collision.Attacker);
            }
        }
    }
}