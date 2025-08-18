using System.Collections.Generic;

namespace _Project.Scripts.Characters.Storages
{
    public class CharactersViewsStorage
    {
        private readonly Dictionary<Character, CharacterView> _map = new();

        public void Register(Character character, CharacterView view) => _map[character] = view;

        public bool TryGet(Character character, out CharacterView view) =>
            _map.TryGetValue(character, out view);

        public bool Unregister(Character character) => _map.Remove(character);
    }
}