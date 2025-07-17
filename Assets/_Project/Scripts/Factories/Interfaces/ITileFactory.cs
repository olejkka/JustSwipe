using UnityEngine.Tilemaps;
using _Project.Scripts.Factories.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Factories.Interfaces
{
    public interface ITileFactory : IFactory
    {
        /// <summary>
        /// Устанавливает и возвращает TileBase в указанной клетке Tilemap.
        /// </summary>
        TileBase CreateTile(Vector2Int boardPosition, TileType tileType);
    }
}