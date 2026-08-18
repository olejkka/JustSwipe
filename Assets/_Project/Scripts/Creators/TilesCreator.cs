using System.Collections.Generic;
using _Project.Scripts.Board;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class TilesCreator
    {
        private readonly EventBus _eventBus;
        private readonly TilesGenerationConfig _config;
        private readonly TilesStorage _storage;


        public TilesCreator(
            EventBus eventBus,
            TilesGenerationConfig config,
            TilesStorage storage
        )
        {
            _eventBus = eventBus;
            _config = config;
            _storage = storage;
        }
        
        public void Create()
        {
            var positions = CollectPositions();
            AssignTypes(positions);
            _eventBus.Publish(new TilesCreatedEvent());
        }

        private List<Vector2Int> CollectPositions()
        {
            var positions = new List<Vector2Int>();

            for (var y = _config.Rect.yMin; y < _config.Rect.yMax; y++)
            for (var x = _config.Rect.xMin; x < _config.Rect.xMax; x++)
            {
                var position = new Vector2Int(x, y);

                var threshold = _config.CoreRect.Contains(new Vector2Int(x, y))
                    ? _config.CoreGenerationChance
                    : _config.CommonGenerationChance;

                if (Random.Range(0, 100) >= threshold)
                    continue;

                positions.Add(position);
            }

            return positions;
        }

        private void AssignTypes(List<Vector2Int> positions)
        {
            var obstacleCount = Mathf.Min(_config.ObstaclesCount, positions.Count);
            Shuffle(positions);

            for (var i = 0; i < positions.Count; i++)
            {
                var type = i < obstacleCount ? TileType.Obstacle : TileType.Default;
                _storage.Add(new Tile(positions[i], type));
            }
        }

        private static void Shuffle(List<Vector2Int> positions)
        {
            for (var i = positions.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (positions[i], positions[j]) = (positions[j], positions[i]);
            }
        }
    }
}
