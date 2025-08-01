using UnityEngine;
using UnityEngine.Tilemaps;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using VContainer;

namespace _Project.Scripts.Factories
{
    public class TileFactory : MonoBehaviour, IFactory
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _tilePrefab;
        
        
        public void CreateTile(Vector2Int position)
        {
            var cell = new Vector3Int(position.x, position.y, 0);
            _tilemap.SetTile(cell, Instantiate(_tilePrefab));
        }
    }
}