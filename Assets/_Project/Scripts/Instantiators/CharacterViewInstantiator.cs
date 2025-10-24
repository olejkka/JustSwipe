using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
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

		
        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);

            if (_characterViewPrefab == null)
            {
                Debug.LogError("Не задан префаб CharacterView в CharacterViewInstantiator");
                return;
            }

            var instance = Instantiate(_characterViewPrefab, worldPos, Quaternion.identity, transform);

            if (instance != null)
            {
                instance.Init(character, _tilemap);
                _charactersViewsStorage.Register(character, instance);
            }
            else
            {
                Debug.LogError("Не удалось инстанцировать CharacterView");
            }
        }
    }
}