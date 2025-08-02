using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersStorage
    {
        private Dictionary<string, CharacterBattleStats> _charactersById = new();

        public void Add(string id, CharacterBattleStats characterBattleStats)
        {
            _charactersById[id] = characterBattleStats;
            
            Debug.Log($"CharactersStorage: Adding {id}");
        }

        public void Remove(string id)
        {
        }

        public CharacterBattleStats Get(string id)
        {
            _charactersById.TryGetValue(id, out var character);
            return character;
        }

        public IReadOnlyDictionary<string, CharacterBattleStats> GetAll()
        {
            return _charactersById;
        }
    }
}