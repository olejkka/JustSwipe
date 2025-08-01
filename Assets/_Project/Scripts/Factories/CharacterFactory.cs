using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Factories
{
    public class CharacterFactory : MonoBehaviour, IFactory
    {
        [SerializeField] private CharactersPrefabsConfig _prefabsConfig;
        [SerializeField] private CharacterStatsConfig _statsConfig;
        [SerializeField] private Tilemap _tilemap;

        private Dictionary<string, GameObject> _prefabsById;

        
        private void Awake()
        {
            _prefabsById = new Dictionary<string, GameObject>();
            
            foreach (var entry in _prefabsConfig.Characters)
                _prefabsById[entry.Id] = entry.Prefab;
        }

        public void CreateCharacterPosition(string id, Vector2Int position)
        {
            if (!_prefabsById.TryGetValue(id, out var prefab))
                return;

            var cell = new Vector3Int(position.x, position.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);
            var characterGO = Instantiate(prefab, worldPos, Quaternion.identity);

            var character = characterGO.GetComponent<CharacterBattleStats>();
            var stats = _statsConfig.Characters.Find(e => e.Id == id);
            
            if (character != null && stats != null)
                character.Init(stats, position);
        }
    }
}