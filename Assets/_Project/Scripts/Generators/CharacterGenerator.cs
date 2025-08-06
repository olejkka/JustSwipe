using System;
using System.Linq;
using _Project.Scripts.Generators.Interfaces;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Generators
{
    public class CharacterGenerator : IGenerator
    {
        public event Action<string, Vector2Int> OnCharacterCreated;
        
        private readonly PositionsStorage _positionsStorage;
        private readonly TilesGenerationConfig _tilesGenerationConfig;


        public CharacterGenerator(
            TilesGenerationConfig tilesGenerationConfig,
            PositionsStorage positionsStorage
        )
        {
            _tilesGenerationConfig = tilesGenerationConfig;
            _positionsStorage = positionsStorage;
        }

        public void GenerateMainCharacter()
        {
            var corePositions = _positionsStorage
                .GetAllPositions()
                .Where(pos => _tilesGenerationConfig.CoreRect.Contains(
                    new Vector2Int(pos.x, pos.y)))
                .ToList();
            
            var spawnPos = corePositions[Random.Range(0, corePositions.Count)];
            
            OnCharacterCreated?.Invoke("Main", spawnPos);
        }
        
        public void GenerateAllyCharacter()
        {
            
        }

        public void GenerateEnemyCharacter()
        {
            var outsidePositions = _positionsStorage
                .GetAllPositions()
                .Where(pos => !_tilesGenerationConfig.CoreRect.Contains(
                    new Vector2Int(pos.x, pos.y)))
                .ToList();
            
            var spawnPos = outsidePositions[Random.Range(0, outsidePositions.Count)];
            
            OnCharacterCreated?.Invoke("Bot_1", spawnPos);
        }
    }
}