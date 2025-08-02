using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : IStartable
    {
        [Inject] private readonly TilesPositionsGenerator _tilesPositionsGenerator;
        [Inject] private readonly TileFactory _tileFactory;

        [Inject] private readonly CharacterPositionGenerator _characterPositionGenerator;
        [Inject] private readonly CharacterFactory _characterFactory;

        public void Start()
        {
            _tilesPositionsGenerator.OnPositionCreated += _tileFactory.CreateTile;
            _characterPositionGenerator.OnCharacterCreated += _characterFactory.Create;

            _tilesPositionsGenerator.Generate();
            _characterPositionGenerator.GenerateMainCharacter();
        }
    }
}