using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Board
{
    public class TilesStorage
    {
        private readonly Dictionary<Vector2Int, Tile> _tiles = new();

        public void Add(Tile tile) => _tiles[tile.Position] = tile;

        public void Remove(Vector2Int pos) => _tiles.Remove(pos);

        public bool Contains(Vector2Int pos) => _tiles.ContainsKey(pos);

        public bool TryGet(Vector2Int pos, out Tile tile) => _tiles.TryGetValue(pos, out tile);

        public IEnumerable<Tile> GetAll() => _tiles.Values;

        public IEnumerable<Vector2Int> GetWalkablePositions()
        {
            foreach (var tile in _tiles.Values)
            {
                if (tile.IsWalkable)
                    yield return tile.Position;
            }
        }
    }
}
