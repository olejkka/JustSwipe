using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;

namespace _Project.Scripts.Characters
{
    public class CharacterDeathHandler : IDisposable
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly Dictionary<Character, Action<int>> _subs = new();

        
        public CharacterDeathHandler(CharactersStorage charactersStorage, CharactersViewsStorage charactersViewsStorage)
        {
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
        }

        public void Register(Character character)
        {
            if (_subs.ContainsKey(character))
                return;

            void OnHealth(int newHealth)
            {
                if (newHealth > 0)
                    return;

                Unregister(character);
                _charactersStorage.Remove(character);

                if (_charactersViewsStorage.TryGet(character, out var view) && view != null)
                {
                    _charactersViewsStorage.Unregister(character);
                    UnityEngine.Object.Destroy(view.gameObject);
                }
            }

            _subs[character] = OnHealth;
            character.OnHealthChanged += OnHealth;
        }

        public void Unregister(Character character)
        {
            if (_subs.TryGetValue(character, out var h))
            {
                character.OnHealthChanged -= h;
                _subs.Remove(character);
            }
        }

        public void Dispose()
        {
            foreach (var kv in _subs)
                kv.Key.OnHealthChanged -= kv.Value;
            _subs.Clear();
        }
    }
}