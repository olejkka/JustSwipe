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

        public void MovePlayerCharacters(Vector2Int vector)
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(Team.Player))
            {
                character.Move(vector);
            }
        }
        
        public void MoveBotCharacters(Vector2Int vector)
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(Team.Bot))
            {
                character.Move(vector);
            }
        }
    }
}