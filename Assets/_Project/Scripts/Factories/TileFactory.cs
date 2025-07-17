using UnityEngine;
using UnityEngine.Tilemaps;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using VContainer;

namespace _Project.Scripts.Factories
{
    public class TileFactory : ITileFactory
    {
        private readonly TileFieldConfig _config;
        private readonly Tilemap _tilemap;

        [Inject]
        public TileFactory(TileFieldConfig config, Tilemap tilemap)
        {
            _config = config;
            _tilemap = tilemap;
        }

        /// <summary>
        /// Устанавливает тайл в указанной клетке Tilemap и возвращает его.
        /// </summary>
        public TileBase CreateTile(Vector2Int boardPosition, TileType tileType)
        {
            var tile = _config.GetTile(tileType);
            if (tile == null)
            {
                Debug.LogWarning($"[TileFactory] Tile for {tileType} is missing in config");
                return null;
            }

            var cell = new Vector3Int(boardPosition.x, boardPosition.y, 0);
            _tilemap.SetTile(cell, tile);
            return tile;
        }
    }
}