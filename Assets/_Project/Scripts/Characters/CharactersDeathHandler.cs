using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Characters
{
    public class CharactersDeathHandler : IDisposable
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly Dictionary<Character, Action<int>> _healthChangeHandlers = new();

        
        public CharactersDeathHandler(
            CharactersStorage charactersStorage,
            CharactersViewsStorage charactersViewsStorage)
        {
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
        }

        public void Register(Character character)
        {
            if (_healthChangeHandlers.ContainsKey(character))
                return;

            var healthHandler = CreateHealthChangeHandler(character);
            _healthChangeHandlers[character] = healthHandler;
            character.OnHealthChanged += healthHandler;
        }
        
        private Action<int> CreateHealthChangeHandler(Character character)
        {
            return newHealth =>
            {
                if (newHealth > 0)
                    return;

                HandleCharacterDeath(character);
            };
        }

        private void HandleCharacterDeath(Character character)
        {
            Unregister(character);
            _charactersStorage.Remove(character);
            DestroyCharacterView(character);
        }

        private void DestroyCharacterView(Character character)
        {
            if (!_charactersViewsStorage.TryGet(character, out var view) || view == null)
                return;

            _charactersViewsStorage.Unregister(character);
            Object.Destroy(view.gameObject);
        }

        private void Unregister(Character character)
        {
            if (!_healthChangeHandlers.TryGetValue(character, out var handler))
                return;

            character.OnHealthChanged -= handler;
            _healthChangeHandlers.Remove(character);
        }
        
        public void Dispose()
        {
            foreach (var (character, handler) in _healthChangeHandlers) 
                character.OnHealthChanged -= handler;
            
            _healthChangeHandlers.Clear();
        }
    }
}