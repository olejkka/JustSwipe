using _Project.Scripts.Characters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Instantiators
{
    public class CharacterViewInstantiator : MonoBehaviour
    {
        [SerializeField] private CharacterView _playerCharacter;
        [SerializeField] private CharacterView _botCharacter;
        [SerializeField] private Tilemap _tilemap;


        public void Instantiate(Character character)
        {
            var cell = new Vector3Int(character.Position.x, character.Position.y, 0);

            var worldPos = _tilemap.CellToWorld(cell);

            var prefab = character.Team == Team.Player
                ? _playerCharacter
                : _botCharacter;

            var instance = Instantiate(prefab, worldPos, Quaternion.identity);
            instance.Init(character, _tilemap);
        }
    }
}