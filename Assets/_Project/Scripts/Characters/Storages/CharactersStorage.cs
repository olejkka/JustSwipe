using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Characters.Storages
{
    public class CharactersStorage : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly List<Character> _characters = new();
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        
        public CharactersStorage(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Start() =>
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);
        
        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
            _characters.Clear();
        }

        public void Add(Character character) => _characters.Add(character);
        public bool Remove(Character character) => _characters.Remove(character);
        
        public IEnumerable<Character> GetCharactersByTeam(Team team) =>
            _characters.Where(character => character.Team == team);

        public IEnumerable<Character> GetAllCharacters() => _characters;

        public IEnumerable<Vector2Int> GetAllPositions() =>
            _characters.Select(character => character.Position);

        private void OnCharacterDied(CharacterDiedEvent e) => 
            Remove(e.Character);
    }
}