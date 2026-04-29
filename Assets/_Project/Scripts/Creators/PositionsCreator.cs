using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Creators
{
    public class PositionsCreator
    {
        private readonly EventBus _eventBus;
        private readonly TilesGenerationConfig _config;
        private readonly TilesPositionsStorage _storage;


        public PositionsCreator(
            EventBus eventBus,
            TilesGenerationConfig config,
            TilesPositionsStorage storage
        )
        {
            _eventBus = eventBus;
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
                _eventBus.Publish(new PositionCreatedEvent(position));
            }
        }
    }
}