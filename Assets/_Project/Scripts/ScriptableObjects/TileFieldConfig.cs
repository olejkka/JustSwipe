// TileFieldConfig.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/Настройки генерации поля тайлов",
        fileName  = "TileFieldConfig"
    )]
    public class TileFieldConfig : ScriptableObject
    {
        [Header("Общие размеры сетки")]
        [Min(1)] [SerializeField] private int _width  = 10;
        [Min(1)] [SerializeField] private int _height = 10;
        
        [Header("Настройки случайности")]
        [Range(0f, 1f)]
        [Tooltip("0 = чистый прямоугольник, 1 = полностью случайная заливка")]
        [SerializeField] private float _randomness = 0.1f;
        [Range(0f, 1f)]
        [Tooltip("При случайном заполнении — вероятность, что клетка будет землёй")]
        [SerializeField] private float _groundProbability = 0.5f;
        
        [Serializable]
        public struct TileEntry
        {
            public TileType  TileType;
            public TileBase  TileAsset;
        }

        [Header("Список тайлов для Tilemap")]
        [SerializeField] private List<TileEntry> _entries = new ();
        
        private Dictionary<TileType, TileBase> _map;
        public IReadOnlyList<TileEntry> Entries => _entries;
        public  int Width  => _width;
        public  int Height => _height;
        public float Randomness      => _randomness;
        public float GroundProbability => _groundProbability;

        
        private void InitMap()
        {
            if (_map != null) 
                return;
            
            _map = new Dictionary<TileType, TileBase>();
            
            foreach (var e in _entries)
                if (e.TileAsset != null)
                    _map[e.TileType] = e.TileAsset;
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