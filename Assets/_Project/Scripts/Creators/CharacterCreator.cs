using System;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators.Generators;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class CharacterCreator
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterInstanceIdGenerator _characterInstanceIdGenerator;


        public CharacterCreator(
            EventBus eventBus,
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage,
            CharactersConfig charactersConfig,
            CharacterInstanceIdGenerator characterInstanceIdGenerator
        )
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
            _charactersConfig = charactersConfig;
            _characterInstanceIdGenerator = characterInstanceIdGenerator;
        }
        
        public void CreateOnRandomPos(string definitionId)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();
            
            var spawnPos = positions[Random.Range(0, positions.Count)];
            
            var entry = _charactersConfig.GetEntryByDefinitionId(definitionId);
            var instanceId = _characterInstanceIdGenerator.Next();
            
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