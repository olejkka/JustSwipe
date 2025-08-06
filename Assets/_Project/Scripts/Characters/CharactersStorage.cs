using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersStorage
    {
        private readonly List<Character> _characters = new();

        
        public void Add(Character character)
        {
            _characters.Add(character);
        }
        
        public IEnumerable<Character> GetAllCharacters()
        {
            return _characters;
        }
        
        public IEnumerable<Vector2Int> GetAllPositions()
        {
            return _characters.Select(character => character.Position);
        }
    }
}