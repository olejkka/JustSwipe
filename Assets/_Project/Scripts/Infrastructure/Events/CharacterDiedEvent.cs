using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.Events
{
    public class CharacterDiedEvent
    {
        public Character Character { get; }
        public CharacterDiedEvent(Character character) => Character = character;
    }
}