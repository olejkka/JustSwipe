using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.Characters
{
    public class CharacterDeathHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly Dictionary<Character, Action<int>> _subs = new();

        
        public CharacterDeathHandler(
            EventBus eventBus,
            CharactersStorage charactersStorage, 
            CharactersViewsStorage charactersViewsStorage
            )
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
        }
        
        public void Start() => 
            _eventBus.Subscribe<CharacterCreatedEvent>(OnCharacterCreated);

        public void Dispose()
        {
            _eventBus.Unsubscribe<CharacterCreatedEvent>(OnCharacterCreated);

            foreach (var kv in _subs)
                kv.Key.OnHealthChanged -= kv.Value;

            _subs.Clear();
        }
        
        private void OnCharacterCreated(CharacterCreatedEvent e) => 
            Register(e.Character);

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

                _eventBus.Publish(new CharacterDiedEvent(character));
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
    }
}