using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Instantiators;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Initializers
{
    public class GameplayInitializer : IStartable, ITickable, IDisposable
    {
        private readonly CharacterCreator _characterCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;

        private readonly CharacterDeathHandler _deathHandler;
        private readonly PhaseHandler _phaseHandler;
        private readonly PositionsCreator _positionsesCreator;
        private readonly GameplayStateMachineCreator _stateMachineCreator;
        private readonly TileInstantiator _tileInstantiator;
        private GameplayStateMachine _fsm;


        public GameplayInitializer(
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            GameplayStateMachineCreator stateMachineCreator,
            PhaseHandler phaseHandler,
            CharacterDeathHandler deathHandler
        )
        {
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _stateMachineCreator = stateMachineCreator;
            _phaseHandler = phaseHandler;
            _deathHandler = deathHandler;
        }

        public void Dispose()
        {
            _positionsesCreator.OnPositionsCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _deathHandler.Register;

            _fsm = null;
        }

        public void Start()
        {
            _positionsesCreator.OnPositionsCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _deathHandler.Register;

            _fsm = _stateMachineCreator.Create();
            _positionsesCreator.Create();
            _characterCreator.Create("Main");
            _characterCreator.Create("Bot_1");
        }

        public void Tick() => _fsm?.UpdateState();
    }
}