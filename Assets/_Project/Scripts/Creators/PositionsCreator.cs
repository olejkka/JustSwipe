using System;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Generators
{
    public class PositionsCreator
    {
        public event Action<Vector2Int> OnPositionCreated;
        
        private readonly TilesGenerationConfig _config;
        private readonly TilesPositionsStorage _storage;


        public PositionsCreator(
            TilesGenerationConfig config,
            TilesPositionsStorage storage
        )
        {
            _config = config;
            _storage = storage;
        }
        
        public void Create()
        {
            for (var y = _config.Rect.yMin; y < _config.Rect.yMax; y++)
            for (var x = _config.Rect.xMin; x < _config.Rect.xMax; x++)
            {
                var position = new Vector2Int(x, y);

                var threshold = _config.CoreRect.Contains(new Vector2Int(x, y))
                    ? _config.CoreGenerationChance
                    : _config.CommonGenerationChance;

                if (Random.Range(0, 100) >= threshold)
                    continue;

                _storage.AddPosition(position);
                OnPositionCreated?.Invoke(position);
            }
        }
    }
}