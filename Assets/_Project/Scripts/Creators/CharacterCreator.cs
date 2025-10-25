// Assets/_Project/Scripts/Creators/CharacterCreator.cs
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
        public event Action<Character> OnCharacterCreated;
        
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly CharactersConfig _charactersConfig;


        public CharacterCreator(
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage,
            CharactersConfig charactersConfig
        )
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
            _charactersConfig = charactersConfig;
        }
        
        public void Create(Team team)
        {
            var positions = _tilesPositionsStorage
                .GetAllPositions()
                .Except(_charactersStorage.GetAllPositions())
                .ToList();

            if (positions.Count == 0)
            {
                Debug.LogWarning($"No available positions to spawn character for team {team}");
                return;
            }

            var spawnPos = positions[Random.Range(0, positions.Count)];
            
            var entry = _charactersConfig.GetEntryByTeam(team);
            if (entry == null)
            {
                Debug.LogError($"No character entry found for team {team}");
                return;
            }

            var character = new Character(spawnPos, team, entry.BaseStats.Copy());

            _charactersStorage.Add(character);
            OnCharacterCreated?.Invoke(character);
        }
    }
}