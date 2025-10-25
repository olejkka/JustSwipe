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
        [SerializeField] private CharacterView _characterViewPrefab;

        [Inject] private CharactersViewsStorage _charactersViewsStorage;
        [Inject] private CharactersConfig _charactersConfig;
        
        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);

            var entry = _charactersConfig.GetEntry(character.CharacterType);
            
            if (entry == null || entry.Sprite == null)
            {
                Debug.LogError($"No sprite found for team {character.Team}");
                return;
            }

            var instance = Instantiate(_characterViewPrefab, worldPos, Quaternion.identity);
            instance.Init(character, _tilemap, entry.Sprite);
            
            _charactersViewsStorage.Register(character, instance);
        }
    }
}