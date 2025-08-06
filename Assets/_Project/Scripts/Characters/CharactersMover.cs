using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMover
    {
        private CharactersStorage _charactersStorage;

        
        public CharactersMover(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public void Move(Vector2Int vector)
        {
            foreach (var character in _charactersStorage.GetAllCharacters())
            {
                character.Move(vector);
            }            
        }
    }
}