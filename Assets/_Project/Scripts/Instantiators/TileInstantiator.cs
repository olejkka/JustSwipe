using System;
using _Project.Scripts.Board;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using BoardTile = _Project.Scripts.Board.Tile;

namespace _Project.Scripts.Instantiators
{
    public class TileInstantiator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilesPrefabsConfig _prefabsConfig;

        [Inject] private EventBus _eventBus;
        [Inject] private TilesStorage _tilesStorage;
        
        private readonly LifetimeDefinition _lifetimeDefinition = new();


        [Inject]
        public void Initialize()
        {
            _eventBus.SubscribeWithLifetime<TilesCreatedEvent>(
                _lifetimeDefinition.Lifetime,
                OnTilesCreated);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        private void OnTilesCreated(TilesCreatedEvent _)
        {
            foreach (var tile in _tilesStorage.GetAll())
                Instantiate(tile);
        }

        public void Instantiate(BoardTile tile)
        {
            var tileAsset = _prefabsConfig.GetRandomTile(tile.Type);
            if (tileAsset == null)
            {
                Debug.LogError($"No tile prefab found for {tile.Type}");
                return;
            }

            var pos = new Vector3Int(tile.Position.x, tile.Position.y, 0);
            _tilemap.SetTile(pos, tileAsset);
        }
    }
}
