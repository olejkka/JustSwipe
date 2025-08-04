using System;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Generators
{
    public class PositionsGenerator : IFactory
    {
        private readonly TilesGenerationConfig _config;
        private readonly PositionsStorage _storage;
        
        public event Action<Vector2Int> OnPositionCreated;

        
        public PositionsGenerator(
            TilesGenerationConfig config,
            PositionsStorage storage
        )
        {
            _config = config;
            _storage = storage;
        }

        public void Generate()
        {
            for (var y = _config.CommonBounds.yMin; y < _config.CommonBounds.yMax; y++)
            for (var x = _config.CommonBounds.xMin; x < _config.CommonBounds.xMax; x++)
            {
                var position = new Vector2Int(x, y);  

                var threshold = _config.CoreBounds.Contains(new Vector3Int(x, y, 0))
                    ? _config.CommonGenerationChance
                    : _config.CoreGenerationChance;
                
                if (Random.Range(0, 100) >= threshold)
                    continue;
                
                _storage.AddPosition(position);
                OnPositionCreated?.Invoke(position);
            }
        }
    }
}