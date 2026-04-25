using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Instantiators
{
    public class CharacterViewInstantiator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private CharacterView _characterViewPrefab;

        [Inject] private CharactersViewsStorage _charactersViewsStorage;
        [Inject] private CharactersConfig _charactersConfig;
        [Inject] private EventBus _eventBus;

        
        [Inject]
        public void Initialize()
        {
            _eventBus.Subscribe<CharacterCreatedEvent>(OnCharacterCreated);
        }

        public void Dispose() => 
            _eventBus.Unsubscribe<CharacterCreatedEvent>(OnCharacterCreated);

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
                entry.Animations.Idle.Length == 0
                )
            {
                Debug.LogError($"No animations found for {character.CharacterType}");
                return;
            }

            var instance = Instantiate(_characterViewPrefab, worldPos, Quaternion.identity);
            instance.Init(character, _tilemap, entry.Animations, _eventBus);
            _charactersViewsStorage.Register(character, instance);
        }
    }
}