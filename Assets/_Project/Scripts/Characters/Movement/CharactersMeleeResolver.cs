using System.Collections.Generic;
using _Project.Scripts.Characters.Health;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersMeleeResolver
    {
        private readonly HealthChangeService _healthChangeService;

        public CharactersMeleeResolver(HealthChangeService healthChangeService)
        {
            _healthChangeService = healthChangeService;
        }

        public void Resolve(IReadOnlyList<MeleeCollision> collisions)
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                var collision = collisions[i];

                _healthChangeService.Enqueue(
                    HealthChangeRequest.Damage(
                        HealthChangeSource.FromCharacter(collision.Attacker),
                        collision.Defender,
                        collision.Attacker.TotalDamage));
            }
        }
    }
}