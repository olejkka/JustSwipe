using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameInitializer : IStartable, IDisposable
    {
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly PositionsCreator _positionsCreator;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly TileInstantiator _tileInstantiator;
        private readonly CharacterSpawnController _characterSpawnController;


        public GameInitializer(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController
        )
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
        }

        public void Start()
        {
            _keyboardInputHandler.OnPressed += _charactersMover.MovePlayerCharacters;
            _swipeInputHandler.OnPressed += _charactersMover.MovePlayerCharacters;
            
            _keyboardInputHandler.OnPressed += _characterSpawnController.HandleInput;
            _swipeInputHandler.OnPressed += _characterSpawnController.HandleInput;
            
            _positionsCreator.OnPositionCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;

            _positionsCreator.Create();
            _characterCreator.Create(Team.Player);
            _characterCreator.Create(Team.Bot);
        }

        public void Dispose()
        {
            _keyboardInputHandler.OnPressed -= _charactersMover.MovePlayerCharacters;
            _swipeInputHandler.OnPressed -= _charactersMover.MovePlayerCharacters;
            
            _keyboardInputHandler.OnPressed -= _characterSpawnController.HandleInput;
            _swipeInputHandler.OnPressed -= _characterSpawnController.HandleInput;

            _positionsCreator.OnPositionCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
        }
    }
}