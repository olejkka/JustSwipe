using System;
using System.Linq;
using _Project.Scripts.Board;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators.Generators;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class CharacterCreator
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesStorage _tilesStorage;
        private readonly CharactersConfig _charactersConfig;
        private readonly InstanceIdGenerator _instanceIdGenerator;


        public CharacterCreator(
            EventBus eventBus,
            CharactersStorage charactersStorage,
            TilesStorage tilesStorage,
            CharactersConfig charactersConfig,
            InstanceIdGenerator instanceIdGenerator
        )
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _tilesStorage = tilesStorage;
            _charactersConfig = charactersConfig;
            _instanceIdGenerator = instanceIdGenerator;
        }
        
        public void CreateOnRandomPos(string definitionId)
        {
            var positions = _tilesStorage
                .GetWalkablePositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();
            
            var spawnPos = positions[Random.Range(0, positions.Count)];
            
            var entry = _charactersConfig.GetEntryByDefinitionId(definitionId);
            var instanceId = _instanceIdGenerator.Next();
            
            if (entry == null)
            {
                Debug.LogError($"No entry found {definitionId}");
                return;
            }

            var character = new Character(definitionId, instanceId, spawnPos, entry.Team, entry.BaseStats.Copy());

            _charactersStorage.Add(character);
            _eventBus.Publish(new CharacterCreatedEvent(character));
        }
    }
}