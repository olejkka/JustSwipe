using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Tiles
{
    public class TilesPositionsStorage
    {
        private readonly Dictionary<Vector2Int, TileType> _tiles = new();

        public void AddPosition(Vector2Int pos, TileType type)
        {
            _tiles[pos] = type;
        }

        public bool Contains(Vector2Int pos) => _tiles.ContainsKey(pos);

        public IReadOnlyCollection<Vector2Int> GetAllPositions() => _tiles.Keys;

        public bool TryGetType(Vector2Int pos, out TileType type)
        {
            return _tiles.TryGetValue(pos, out type);
        }
    }
}