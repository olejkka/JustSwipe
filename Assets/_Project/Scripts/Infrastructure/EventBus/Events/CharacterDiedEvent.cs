using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class CharacterDiedEvent
    {
        public Character Character { get; }
        public Character Killer { get; }
        public CharacterDiedEvent(Character character, Character killer = null)
        {
            Character = character;
            Killer = killer;
        }
    }
}