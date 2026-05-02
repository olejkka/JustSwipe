using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.Characters.Storages
{
    public class CharactersViewsStorage : IStartable,  IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly Dictionary<Character, CharacterView> _map = new();
        private readonly LifetimeDefinition _lifetimeDefinition = new();


        public CharactersViewsStorage(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Start() =>
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);
        
        public void Dispose()
        {
            _lifetimeDefinition.Terminate();

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
                view.PlayDeath(() => UnityEngine.Object.Destroy(view.gameObject));
            }
        }
    }
}