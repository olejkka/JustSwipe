using System;
using System.Collections.Generic;
using _Project.Scripts.Board;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Tile Highlight",
        fileName = "Tile Highlight"
    )]
    public class TileHighlightConfig : ScriptableObject
    {
        [SerializeField] private TileBase _overlayTile;
        [SerializeField] private List<TileHighlightEntry> _entries = new();

        public TileBase OverlayTile => _overlayTile;


        public TileHighlightEntry Get(TileHighlightType type)
        {
            for (var i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Type == type)
                    return _entries[i];
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }


        [Serializable]
        public class TileHighlightEntry
        {
            public TileHighlightType Type;
            public Color Color;
            public float Duration;
        }
    }
}
