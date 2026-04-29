using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Instantiators
{
    public class TileInstantiator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilesPrefabsConfig _prefabsConfig;

        [Inject] private EventBus _eventBus;

        [Inject]
        public void Initialize()
        {
            _eventBus.Subscribe<PositionCreatedEvent>(OnPositionCreated);
        }

        public void Dispose()
        {
            _eventBus?.Unsubscribe<PositionCreatedEvent>(OnPositionCreated);
        }

        private void OnPositionCreated(PositionCreatedEvent e)
        {
            Instantiate(e.Position);
        }

        public void Instantiate(Vector2Int position)
        {
            var pos = new Vector3Int(position.x, position.y, 0);
            var tile = PickTile();
            _tilemap.SetTile(pos, tile);
        }

        private TileBase PickTile()
        {
            foreach (var entry in _prefabsConfig.Entries)
            {
                if (entry.TileType == TileType.Ground)
                    continue;
                if (Random.Range(0, 100) < entry.Chance)
                    return entry.TileAsset;
            }

            return _prefabsConfig.GetTile(TileType.Ground);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}