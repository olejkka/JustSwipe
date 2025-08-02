using System;
using System.Linq;
using _Project.Scripts.Generators.Interfaces;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;

namespace _Project.Scripts.Generators
{
    public class CharacterPositionGenerator : IGenarator
    {
        private readonly CharacterStatsConfig _statsConfig;
        private readonly TilesPositionsStorage _tilesStorage;

        public event Action<string, Vector2Int> OnCharacterCreated;

        
        public CharacterPositionGenerator(
            CharacterStatsConfig statsConfig,
            TilesPositionsStorage tilesStorage
            )
        {
            _statsConfig = statsConfig;
            _tilesStorage = tilesStorage;
        }

        public void GenerateMainCharacter()
        {
            const string mainId = "Main";

            var allPositions = _tilesStorage.GetAllPositions().ToList();

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;
            
            foreach (var p in allPositions)
            {
                if (p.x < minX) minX = p.x;
                if (p.x > maxX) maxX = p.x;
                if (p.y < minY) minY = p.y;
                if (p.y > maxY) maxY = p.y;
            }

            var center = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
            float bestDist = float.MaxValue;
            Vector2Int bestPos = allPositions[0];
            
            foreach (var p in allPositions)
            {
                if (!_tilesStorage.TryGetType(p, out var type)) 
                    continue;
                
                if (type != TileType.Ground) 
                    continue;

                float dsq = (p - center).sqrMagnitude;
                
                if (dsq < bestDist)
                {
                    bestDist = dsq;
                    bestPos  = p;
                }
            }

            OnCharacterCreated?.Invoke(mainId, bestPos);
        }

        public void GenerateAllyCharacter() { }
        public void GenerateEnemyCharacter() { }
    }
}
