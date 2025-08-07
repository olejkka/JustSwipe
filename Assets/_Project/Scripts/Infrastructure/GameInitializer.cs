using System;
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
    public class GameInitializer : IStartable, IDisposable
    {
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly PositionsCreator _positionsCreator;
        private readonly CharacterCreator _characterCreator;
        private readonly TileInstantiator _tileInstantiator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly CharactersMover _charactersMover;

        
        public GameInitializer(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            CharactersMover charactersMover)
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _charactersMover = charactersMover;
        }

        public void Start()
        {
            _keyboardInputHandler.OnPressed += _charactersMover.MovePlayerCharacters;
            _swipeInputHandler.OnPressed += _charactersMover.MovePlayerCharacters;
            _positionsCreator.OnPositionCreated += _tileInstantiator.Instantiate;
            _positionsCreator.Create();
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.Create(Team.Player);
            _characterCreator.Create(Team.Bot);
        }

        public void Dispose()
        {
            _keyboardInputHandler.OnPressed -= _charactersMover.MovePlayerCharacters;
            _swipeInputHandler.OnPressed -= _charactersMover.MovePlayerCharacters;
            _positionsCreator.OnPositionCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
        }
    }
}