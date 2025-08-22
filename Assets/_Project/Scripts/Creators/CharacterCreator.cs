using System;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class CharacterCreator
    {
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharacterStatsConfig _characterStatsConfig;


        public CharacterCreator(
            CharacterStatsConfig characterStatsConfig,
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage
        )
        {
            _characterStatsConfig = characterStatsConfig;
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
        }

        public event Action<Character> OnCharacterCreated;

        public void Create(string characterId)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();

            var spawnPos = positions[Random.Range(0, positions.Count)];

            var statsEntry = _characterStatsConfig.CharacterStatsEntries
                .FirstOrDefault(entry => entry.Id == characterId);

            if (statsEntry == null)
            {
                Debug.LogError($"Не найдены статы для персонажа: {characterId}");
                return;
            }

            var character = new Character(
                characterId,
                spawnPos,
                statsEntry.team,
                statsEntry.BaseMaxHealth,
                statsEntry.BaseAttackDamage
            );

            _charactersStorage.Add(character);
            OnCharacterCreated?.Invoke(character);
        }
    }
}