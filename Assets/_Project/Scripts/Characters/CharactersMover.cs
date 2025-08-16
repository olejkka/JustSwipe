using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMover
    {
        private CharactersStorage _charactersStorage;
        
        public event Action OnMove;

        
        public CharactersMover(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public void Move(Vector2Int vector, Team team)
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(team))
                character.Move(vector);
            
            OnMove?.Invoke();
        }
    }
}