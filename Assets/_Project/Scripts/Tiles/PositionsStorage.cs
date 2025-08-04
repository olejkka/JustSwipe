using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Tiles
{
    public class PositionsStorage
    {
        private readonly HashSet<Vector2Int> _tiles = new();

        public void AddPosition(Vector2Int pos)
        {
            _tiles.Add(pos);
        }

        // public bool Contains(Vector2Int pos) => _tiles.ContainsKey(pos);

        public IReadOnlyCollection<Vector2Int> GetAllPositions() => _tiles;

        // public bool TryGetType(Vector2Int pos, out TileType type)
        // {
        //     return _tiles.TryGetValue(pos, out type);
        // }
    }
}