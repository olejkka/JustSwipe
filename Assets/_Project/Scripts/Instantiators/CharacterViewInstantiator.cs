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
        [SerializeField] private CharacterView _characterView;

        [Inject] private CharactersViewsStorage _charactersViewsStorage;


        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);

            var worldPos = _tilemap.CellToWorld(cell);

            var instance = Instantiate(_characterView, worldPos, Quaternion.identity);

            instance.Init(character, _tilemap);
            _charactersViewsStorage.Register(character, instance);
        }
    }
}