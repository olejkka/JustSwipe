using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class CharacterAttackHighlightRequestedEvent
    {
        public Character Character { get; }

        public CharacterAttackHighlightRequestedEvent(Character character) => Character = character;
    }
}
