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


        public CharacterCreator(
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage
        )
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
        }

        public event Action<Character> OnCharacterCreated;

        public void Create(CharacterConfig characterConfig)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();

            var spawnPos = positions[Random.Range(0, positions.Count)];

            var character = new Character(spawnPos, characterConfig);

            _charactersStorage.Add(character);
            OnCharacterCreated?.Invoke(character);
        }
    }
}