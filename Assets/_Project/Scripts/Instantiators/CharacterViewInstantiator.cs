using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace _Project.Scripts.Instantiators
{
    public class CharacterViewInstantiator : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        
        [Inject] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [Inject] private CharactersViewsStorage _charactersViewsStorage;

        
        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);

            var worldPos = _tilemap.CellToWorld(cell);

            var prefabEntry = _charactersPrefabsConfig.CharacterPrefabEntries
                .FirstOrDefault(entry => entry.Id == character.Id);

            if (prefabEntry?.Prefab == null)
            {
                Debug.LogError($"Не найден префаб для персонажа с ID: {character.Id}");
                return;
            }

            var instance = Instantiate(prefabEntry.Prefab, worldPos, Quaternion.identity);

            if (instance.TryGetComponent<CharacterView>(out var characterView))
            {
                characterView.Init(character, _tilemap);
                _charactersViewsStorage.Register(character, characterView);
            }
            else
            {
                Debug.LogError($"Префаб {prefabEntry.Prefab.name} не содержит компонент CharacterView");
            }
        }
    }
}