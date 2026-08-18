using UnityEngine;

namespace _Project.Scripts.Board
{
    public class Tile
    {
        public Vector2Int Position { get; }
        public TileType Type { get; }
        public bool IsWalkable => Type != TileType.Obstacle;

        public Tile(Vector2Int position, TileType type)
        {
            Position = position;
            Type = type;
        }
    }
}
