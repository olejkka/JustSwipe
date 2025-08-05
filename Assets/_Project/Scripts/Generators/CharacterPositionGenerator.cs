using System;
using System.Linq;
using _Project.Scripts.Generators.Interfaces;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Generators
{
    public class CharacterPositionGenerator : IGenerator
    {
        private readonly TilesGenerationConfig _tilesGenerationConfig;
        private readonly PositionsStorage _positionsStorage;

        public event Action<string, Vector2Int> OnCharacterCreated;

        
        public CharacterPositionGenerator(
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
                .Where(pos => _tilesGenerationConfig.CoreBounds.Contains(
                    new Vector2Int(pos.x, pos.y)))
                .ToList();
            
            var spawnPos = corePositions[Random.Range(0, corePositions.Count)];
            
            OnCharacterCreated?.Invoke("Main", spawnPos);
        }

        public void GenerateAllyCharacter() { }
        public void GenerateEnemyCharacter() { }
    }
}
