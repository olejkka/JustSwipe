using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
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
        private readonly PositionsCreator _positionsesCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly TileInstantiator _tileInstantiator;
        
        private readonly PhaseHandler _phaseHandler;
        
        private readonly GameplayStateMachineCreator _stateMachineCreator;
        private GameplayStateMachine _fsm;

        
        public GameplayInitializer(
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            GameplayStateMachineCreator stateMachineCreator,
            PhaseHandler phaseHandler
        )
        {
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _stateMachineCreator = stateMachineCreator;
            _phaseHandler = phaseHandler;
        }

        public void Start()
        {
            _positionsesCreator.OnPositionsCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;

            // _fsm = _stateMachineCreator.Create();
            _positionsesCreator.Create();
            _characterCreator.Create(Team.Player);
            _characterCreator.Create(Team.Bot);
        }
        
        public void Tick()
        {
            _fsm?.UpdateState();
        }

        public void Dispose()
        {
            _positionsesCreator.OnPositionsCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
            
            _fsm = null;
        }
    }
}