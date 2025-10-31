using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Generators;
using _Project.Scripts.Wallet;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayEntryPoint : IStartable
    {
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsesCreator;
        private readonly Money _money;
        
        
        public GameplayEntryPoint(
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            Money money
        )
        {
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _money = money;
        }

        public void Start()
        {
            _positionsesCreator.Create();
            _money.SetAmount(10);
            _characterCreator.CreateOnRandomPos(CharacterType.Main);
            _characterCreator.CreateOnRandomPos(CharacterType.Bot_1);
        }
    }
}