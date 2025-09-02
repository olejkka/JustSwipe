using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Tiles;

namespace _Project.Scripts.Characters
{
    public class CharactersPositionValidator
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        

        public CharactersPositionValidator(
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage
            )
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
        }

        public void ValidateAllCharacters()
        {
            var characters = _charactersStorage.GetAllCharacters().ToArray();
            
            foreach (var character in characters)
                if (!_tilesPositionsStorage.Contains(character.Position))
                    character.TakeDamage(character._stats.Health);
        }
    }
}