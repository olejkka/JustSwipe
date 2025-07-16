using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Factories
{
    public class TileFactory : ITileFactory, IDisposable
    {
        private readonly TileFieldConfig _config;
        private readonly ObjectPool<GameObject> _pool;

        public TileFactory(TileFieldConfig config, Transform parent = null)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            int capacity = _config.Width * _config.Height;
            _pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var go = new GameObject("Tile");
                    if (parent != null)
                        go.transform.SetParent(parent, false);
                    go.AddComponent<SpriteRenderer>();
                    return go;
                },
                actionOnGet: go => go.SetActive(true),
                actionOnRelease: go => go.SetActive(false),
                actionOnDestroy: go => UnityEngine.Object.Destroy(go),
                collectionCheck: false,
                defaultCapacity: capacity,
                maxSize: capacity * 2
            );
        }

        public GameObject CreateTile(Vector2Int position, TileType type)
        {
            var tileAsset = _config.GetTile(type);
            
            if (tileAsset is Tile tile)
            {
                var go = _pool.Get();
                go.name = $"{type}_{position.x}_{position.y}";
                go.transform.position = new Vector3(position.x, position.y, 0);

                var sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = tile.sprite;
                
                return go;
            }

            throw new InvalidOperationException($"В конфиге нет Tile-ассета для {type}");
        }

        public void ReleaseTile(GameObject tile) => _pool.Release(tile);

        public void Dispose() => _pool.Dispose();
    }
}
