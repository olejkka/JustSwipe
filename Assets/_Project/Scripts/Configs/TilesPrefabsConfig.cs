using System;
using System.Collections.Generic;
using _Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay/TilesPrefabsConfig",
        fileName = "TilesPrefabsConfig"
    )]
    public class TilesPrefabsConfig : ScriptableObject
    {
        [SerializeField] private List<TileEntry> _entries = new();

        private Dictionary<TileType, TileBase> _map;

        public IReadOnlyList<TileEntry> Entries => _entries;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _map = null;
        }
#endif

        private void InitMap()
        {
            if (_map != null)
                return;

            _map = new Dictionary<TileType, TileBase>();

            foreach (var entry in _entries)
                if (entry.TileAsset != null)
                    _map[entry.TileType] = entry.TileAsset;
        }

        public TileBase GetTile(TileType type)
        {
            InitMap();
            return _map.TryGetValue(type, out var tile) ? tile : null;
        }

        [Serializable]
        public class TileEntry
        {
            public TileType TileType;
            public TileBase TileAsset;
            [Range(0, 100)] public int Chance;
        }
    }
}