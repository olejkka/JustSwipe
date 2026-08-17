using System.Linq;
using _Project.Scripts.Board;
using _Project.Scripts.Characters.Health;
using _Project.Scripts.Characters.Storages;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersPositionResolver
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly HealthChangeService _healthChangeService;

        
        public CharactersPositionResolver(
            CharactersStorage charactersStorage,
            TilesPositionsStorage tilesPositionsStorage,
            HealthChangeService healthChangeService)
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
            _healthChangeService = healthChangeService;
        }

        public void Resolve()
        {
            var characters = _charactersStorage.GetAllCharacters().ToArray();

            for (int i = 0; i < characters.Length; i++)
            {
                var character = characters[i];

                if (!_tilesPositionsStorage.Contains(character.Position))
                    _healthChangeService.Enqueue(
                        HealthChangeRequest.Damage(
                            HealthChangeSource.None,
                            character,
                            character.Health + character.BonusHealth));
            }
        }
    }
}