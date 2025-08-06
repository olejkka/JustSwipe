using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : IStartable
    {
        [Inject] private readonly KeyboardInputHandler _keyboardInputHandler;
        [Inject] private readonly SwipeInputHandler _swipeInputHandler;
        
        [Inject] private readonly CharacterCreator _characterCreator;
        [Inject] private readonly PositionsCreator _positionsCreator;
        
        [Inject] private readonly CharacterViewInstantiator _characterViewInstantiator;
        [Inject] private readonly TileInstantiator _tileInstantiator;
        
        [Inject] private readonly CharactersMover _charactersMover;
        

        public void Start()
        {
            _keyboardInputHandler.OnPressed += _charactersMover.Move;
            _swipeInputHandler.OnPressed += _charactersMover.Move;
                
            _positionsCreator.OnPositionCreated += _tileInstantiator.Instantiate;
            _positionsCreator.Create();

            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.Create(Team.Player);
            _characterCreator.Create(Team.Bot);
        }
    }
}