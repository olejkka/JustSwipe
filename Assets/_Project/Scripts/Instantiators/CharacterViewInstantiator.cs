using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace _Project.Scripts.Instantiators
{
    public class CharacterViewInstantiator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private CharacterView _characterViewPrefab;

        [Inject] private CharactersViewsStorage _charactersViewsStorage;
        [Inject] private CharactersConfig _charactersConfig;
        [Inject] private EventBus _eventBus;
        
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        
        [Inject]
        public void Initialize()
        {
            _eventBus.SubscribeWithLifetime<CharacterCreatedEvent>(_lifetimeDefinition.Lifetime, OnCharacterCreated);
        }

        public void Dispose() => 
            _lifetimeDefinition.Terminate();

        private void OnCharacterCreated(CharacterCreatedEvent e) => 
            Instantiate(e.Character);

        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);

            var entry = _charactersConfig.GetEntryByDefinitionId(character.DefinitionId);
            
            if (
                entry == null || 
                entry.Animations == null || 
                entry.Animations.Idle == null || 
                entry.Animations.Idle.Length == 0)
            {
                Debug.LogError($"No animations found for {character.DefinitionId}");
                return;
            }

            var instance = Instantiate(_characterViewPrefab, worldPos, Quaternion.identity);
            instance.Init(character, _tilemap, entry.Animations, _eventBus);
            _charactersViewsStorage.Register(character, instance);
        }
    }
}