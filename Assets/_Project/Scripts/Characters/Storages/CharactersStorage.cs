using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Characters.Storages
{
    public class CharactersStorage
    {
        private readonly List<Character> _characters = new();

        
        public void Add(Character character) => _characters.Add(character);
        public bool Remove(Character character) => _characters.Remove(character);
        
        public IEnumerable<Character> GetCharactersByTeam(Team team) =>
            _characters.Where(character => character.Team == team);

        public IEnumerable<Character> GetAllCharacters() => _characters;

        public IEnumerable<Vector2Int> GetAllPositions() =>
            _characters.Select(character => character.Position);
    }
}