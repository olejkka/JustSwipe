using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : IStartable
    {
        [Inject] private readonly PositionsGenerator _positionsGenerator;
        [Inject] private readonly TileFactory _tileFactory;

        [Inject] private readonly CharacterGenerator _characterGenerator;
        [Inject] private readonly CharacterFactory _characterFactory;

        public void Start()
        {
            _positionsGenerator.OnPositionCreated += _tileFactory.Create;
            _positionsGenerator.Generate();
            
            _characterGenerator.OnCharacterCreated += _characterFactory.Create;
            _characterGenerator.GenerateMainCharacter();
            _characterGenerator.GenerateEnemyCharacter();
        }
    }
}