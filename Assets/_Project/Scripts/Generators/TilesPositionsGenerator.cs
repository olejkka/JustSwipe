using System;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Generators
{
    public class TilesPositionsGenerator : IFactory
    {
        public event Action<Vector2Int> OnPositionCreated;

        private TilesGenerationSettingsConfig _config;
        private TilesPositionsStorage _storage;

        public TilesPositionsGenerator(
            TilesGenerationSettingsConfig config,
            TilesPositionsStorage storage
        )
        {
            _config = config;
            _storage = storage;
        }

        public void Generate()
        {
            int fieldWidth = _config.MaxRoomWidth;
            int fieldHeight = _config.MaxRoomHeight;

            int roomW = Random.Range(_config.MinRoomWidth, fieldWidth + 1);
            int roomH = Random.Range(_config.MinRoomHeight, fieldHeight + 1);

            int x0 = Random.Range(0, fieldWidth - roomW + 1);
            int y0 = Random.Range(0, fieldHeight - roomH + 1);

            for (int x = 0; x < fieldWidth; x++)
            for (int y = 0; y < fieldHeight; y++)
            {
                bool inside = x >= x0 && x < x0 + roomW
                                      && y >= y0 && y < y0 + roomH;

                if (!inside)
                    continue;

                if (Random.value > _config.Randomness)
                    continue;

                Vector2Int pos = new Vector2Int(x, y);
                _storage.AddPosition(pos, TileType.Ground);
                OnPositionCreated?.Invoke(pos);
            }
        }
    }
}