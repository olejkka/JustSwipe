using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/TilesPrefabsConfig",
        fileName = "TilesPrefabsConfig"
    )]
    public class TilesPrefabsConfig : ScriptableObject
    {
        [Serializable]
        public class TileEntry
        {
            public TileType TileType;
            public TileBase TileAsset;
        }

        [SerializeField] private List<TileEntry> _entries = new();

        private Dictionary<TileType, TileBase> _map;

        public IReadOnlyList<TileEntry> Entries => _entries;

        private void InitMap()
        {
            if (_map != null)
                return;

            _map = new Dictionary<TileType, TileBase>();
            
            foreach (var entry in _entries)
            {
                if (entry.TileAsset != null)
                    _map[entry.TileType] = entry.TileAsset;
            }
        }

        public TileBase GetTile(TileType type)
        {
            InitMap();
            return _map.TryGetValue(type, out var tile) ? tile : null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _map = null;
        }
#endif
    }
}