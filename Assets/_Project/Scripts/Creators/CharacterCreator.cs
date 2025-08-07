using System;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Tiles;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class CharacterCreator
    {
        public event Action<Character> OnCharacterCreated;
        
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;


        public CharacterCreator(
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage
        )
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
        }
        
        public void Create(Team player)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();

            var spawnPos = positions[Random.Range(0, positions.Count)];

            var character = new Character(player, 10, 1, spawnPos);

            _charactersStorage.Add(character);
            OnCharacterCreated?.Invoke(character);
        }
    }
}