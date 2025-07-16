using UnityEngine;

namespace _Project.Scripts.Factories.Interfaces
{
    public interface ITileFactory : IFactory
    {
        GameObject CreateTile(Vector2Int boardPosition, TileType tileType);
    }
}