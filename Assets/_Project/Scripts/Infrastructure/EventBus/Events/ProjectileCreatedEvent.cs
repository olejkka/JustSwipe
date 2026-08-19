using _Project.Scripts.Characters.Projectiles;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class ProjectileCreatedEvent
    {
        public Projectile Projectile { get; }

        public ProjectileCreatedEvent(Projectile projectile) => Projectile = projectile;
    }
}
