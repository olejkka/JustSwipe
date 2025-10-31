using System;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.ScriptableObjects;
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


        public CharacterCreator(
            EventBus eventBus,
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage,
            CharactersConfig charactersConfig
        )
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
            _charactersConfig = charactersConfig;
        }
        
        public void CreateOnRandomPos(CharacterType characterType)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();

            if (positions.Count == 0)
            {
                Debug.LogWarning($"No available positions to spawn character {characterType}");
                return;
            }

            var spawnPos = positions[Random.Range(0, positions.Count)];
            
            var entry = _charactersConfig.GetEntry(characterType);
            
            if (entry == null)
            {
                Debug.LogError($"No entry found {characterType}");
                return;
            }

            var character = new Character(spawnPos, characterType, entry.Team, entry.CharacterBaseStats.Copy());

            _charactersStorage.Add(character);
            _eventBus.Publish(new CharacterCreatedEvent(character));
        }
    }
}