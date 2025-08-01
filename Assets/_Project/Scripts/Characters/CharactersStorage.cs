using System.Collections.Generic;

namespace _Project.Scripts.Characters
{
    public class CharactersStorage
    {
        private Dictionary<string, CharacterBattleStats> _charactersById = new();

        public void Add(string id, CharacterBattleStats characterBattleStats)
        {
            _charactersById[id] = characterBattleStats;
        }

        // public void Remove(string id)
        // {
        //     if (_charactersById.TryGetValue(id, out var character))
        //     {
        //         GameObject.Destroy(character.gameObject); // или pooled
        //         _charactersById.Remove(id);
        //     }
        // }

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