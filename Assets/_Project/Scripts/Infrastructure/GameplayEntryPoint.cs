using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Instantiators;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayEntryPoint : IStartable, ITickable, IDisposable
    {
        private readonly GameplayStateMachineCreator _stateMachineCreator;
        private GameplayStateMachine _fsm;
        
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsesCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly TileInstantiator _tileInstantiator;
        
        private readonly CharacterDeathHandler _deathHandler;
        
        
        public GameplayEntryPoint(
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            GameplayStateMachineCreator stateMachineCreator,
            CharacterDeathHandler deathHandler
        )
        {
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _stateMachineCreator = stateMachineCreator;
            _deathHandler = deathHandler;
        }

        public void Start()
        {
            _positionsesCreator.OnPositionsCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _deathHandler.Register;

            _fsm = _stateMachineCreator.Create();
            _positionsesCreator.Create();
            _characterCreator.Create(CharacterType.Main);
            _characterCreator.Create(CharacterType.Bot_1);
        }
        
        public void Tick()
        {
            _fsm?.UpdateState();
        }

        public void Dispose()
        {
            _positionsesCreator.OnPositionsCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _deathHandler.Register;
            
            _fsm = null;
        }
    }
}