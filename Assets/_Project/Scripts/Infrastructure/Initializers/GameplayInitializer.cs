using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Factories;
using _Project.Scripts.FSM;
using _Project.Scripts.Generators;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Initializers
{
    public class GameplayInitializer : IStartable, ITickable, IDisposable
    {
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly TileInstantiator _tileInstantiator;
        
        private readonly GameplayStateMachineCreator _stateMachineCreator;
        private GameplayStateMachine _fsm;

        
        public GameplayInitializer(
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            GameplayStateMachineCreator stateMachineCreator
        )
        {
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _stateMachineCreator = stateMachineCreator;
        }

        public void Start()
        {
            _positionsCreator.OnPositionCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;

            _fsm = _stateMachineCreator.Create();
            _positionsCreator.Create();
            _characterCreator.Create(Team.Player);
            _characterCreator.Create(Team.Bot);
        }
        
        public void Tick()
        {
            _fsm?.UpdateState();
        }

        public void Dispose()
        {
            _positionsCreator.OnPositionCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
            
            _fsm = null;
        }
    }
}