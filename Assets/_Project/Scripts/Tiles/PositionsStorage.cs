using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Tiles
{
    public class PositionsStorage
    {
        private readonly HashSet<Vector2Int> _tiles = new();

        public void AddPosition(Vector2Int pos) => _tiles.Add(pos);
        
        public void RemovePosition(Vector2Int pos) => _tiles.Remove(pos);

        public bool Contains(Vector2Int pos) => _tiles.Contains(pos);

        public IReadOnlyCollection<Vector2Int> GetAllPositions() => _tiles;
    }
}