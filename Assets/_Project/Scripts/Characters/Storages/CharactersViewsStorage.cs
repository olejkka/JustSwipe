using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.Characters.Storages
{
    public class CharactersViewsStorage : IStartable,  IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly Dictionary<Character, CharacterView> _map = new();


        public CharactersViewsStorage(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Start() =>
            _eventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);
        
        public void Dispose()
        {
            _eventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);

            foreach (var kv in _map)
            {
                if (kv.Value != null)
                    UnityEngine.Object.Destroy(kv.Value.gameObject);
            }

            _map.Clear();
        }

        public void Register(Character character, CharacterView view) => _map[character] = view;

        public bool TryGet(Character character, out CharacterView view) =>
            _map.TryGetValue(character, out view);

        public bool Unregister(Character character) => _map.Remove(character);
        
        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (TryGet(e.Character, out var view) && view != null)
            {
                Unregister(e.Character);
                UnityEngine.Object.Destroy(view.gameObject);
            }
        }
    }
}