using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class CharacterCreatedEvent
    {
        public Character Character { get; }
        public CharacterCreatedEvent(Character character) => Character = character;
    }
}