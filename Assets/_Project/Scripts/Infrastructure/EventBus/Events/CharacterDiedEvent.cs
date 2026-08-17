using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Health;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class CharacterDiedEvent
    {
        public Character Character { get; }
        public HealthChangeSource Source { get; }

        public CharacterDiedEvent(Character character, HealthChangeSource source)
        {
            Character = character;
            Source = source;
        }
    }
}