using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Economy;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.EntryPoints
{
    public class GameplayEntryPoint : IStartable, ITickable, IDisposable
    {
        private IReadOnlyList<CharacterConfig> _characterConfigs;
        private readonly CharacterCreator _characterCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;
        private readonly CharactersDeathHandler _deathHandler;
        private readonly PositionsCreator _positionsesCreator;
        private readonly GameplayStateMachineCreator _stateMachineCreator;
        private readonly TileInstantiator _tileInstantiator;
        private readonly PlayerMoney _playerMoney;
        
        private GameplayStateMachine _fsm;


        public GameplayEntryPoint(
            IReadOnlyList<CharacterConfig> characterConfigs,
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            GameplayStateMachineCreator stateMachineCreator,
            CharactersDeathHandler deathHandler,
            PlayerMoney playerMoney
        )
        {
            _characterConfigs = characterConfigs;
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _stateMachineCreator = stateMachineCreator;
            _deathHandler = deathHandler;
            _playerMoney = playerMoney;
        }
        
        public void Start()
        {
            _positionsesCreator.OnPositionsCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _deathHandler.Register;

            _fsm = _stateMachineCreator.Create();
            
            _playerMoney.SetMoney(10);
            _positionsesCreator.Create();
            CreateCharacters();
        }

        public void Tick() => _fsm?.UpdateState();
        
        private void CreateCharacters()
        {
            _characterCreator.Create(_characterConfigs.First(config => config.Team == Team.Player));
            _characterCreator.Create(_characterConfigs.First(config => config.Team == Team.Bot));
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