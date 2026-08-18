using System;
using System.Collections.Generic;
using _Project.Scripts.Board;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Tiles Prefabs",
        fileName = "Tiles Prefabs"
    )]
    public class TilesPrefabsConfig : ScriptableObject
    {
        [SerializeField] private List<TileEntry> _entries = new();

        public IReadOnlyList<TileEntry> Entries => _entries;

        
        public TileBase GetRandomTile(TileType type)
        {
            var totalChance = 0;

            foreach (var entry in _entries)
            {
                if (!IsCandidate(entry, type))
                    continue;
                
                totalChance += entry.Chance;
            }

            if (totalChance <= 0)
                return null;

            var roll = Random.Range(0, totalChance);

            foreach (var entry in _entries)
            {
                if (!IsCandidate(entry, type))
                    continue;
                
                if (roll < entry.Chance)
                    return entry.TileAsset;
                
                roll -= entry.Chance;
            }

            return null;
        }

        private static bool IsCandidate(TileEntry entry, TileType type) =>
            entry.TileType == type && entry.TileAsset != null && entry.Chance > 0;

        
        [Serializable]
        public class TileEntry
        {
            public TileType TileType;
            public TileBase TileAsset;
            [Range(0, 100)] public int Chance;
        }
    }
}
