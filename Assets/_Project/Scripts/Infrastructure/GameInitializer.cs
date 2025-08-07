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
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotInputHandler _botInputHandler;
        
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsCreator;
        
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly TileInstantiator _tileInstantiator;
        
        private readonly CharactersMover _charactersMover;
        
        private readonly CharacterSpawnController _characterSpawnController;


        public GameInitializer(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            BotInputHandler botInputHandler
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
            _botInputHandler = botInputHandler;
        }

        public void Start()
        {
            _keyboardInputHandler.OnPressed += _charactersMover.Move;
            _swipeInputHandler.OnPressed += _charactersMover.Move;
            _botInputHandler.OnPressed += _charactersMover.Move;
            
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
            _keyboardInputHandler.OnPressed -= _charactersMover.Move;
            _swipeInputHandler.OnPressed -= _charactersMover.Move;
            _botInputHandler.OnPressed -= _charactersMover.Move;
            
            _keyboardInputHandler.OnPressed -= _characterSpawnController.HandleInput;
            _swipeInputHandler.OnPressed -= _characterSpawnController.HandleInput;

            _positionsCreator.OnPositionCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
        }
    }
}