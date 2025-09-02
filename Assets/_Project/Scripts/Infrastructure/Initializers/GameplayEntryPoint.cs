using System;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Economy;
using _Project.Scripts.Generators;
using _Project.Scripts.Instantiators;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Initializers
{
    public class GameplayEntryPoint : IStartable, IDisposable
    {
        private readonly CharacterConfig[] _characterConfigs;
        private readonly CharacterCreator _characterCreator;
        private readonly CharacterViewInstantiator _characterViewInstantiator;

        private readonly CharactersDeathHandler _deathHandler;
        private readonly PlayerMoney _playerMoney;
        private readonly PositionsCreator _positionsesCreator;
        private readonly TileInstantiator _tileInstantiator;


        public GameplayEntryPoint(
            CharacterConfig[] characterConfigs,
            PositionsCreator positionsesCreator,
            CharacterCreator characterCreator,
            TileInstantiator tileInstantiator,
            CharacterViewInstantiator characterViewInstantiator,
            CharactersDeathHandler deathHandler,
            PlayerMoney playerMoney
        )
        {
            _characterConfigs = characterConfigs;
            _positionsesCreator = positionsesCreator;
            _characterCreator = characterCreator;
            _tileInstantiator = tileInstantiator;
            _characterViewInstantiator = characterViewInstantiator;
            _deathHandler = deathHandler;
            _playerMoney = playerMoney;
        }

        public void Dispose()
        {
            _positionsesCreator.OnPositionsCreated -= _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated -= _deathHandler.Register;
        }

        public void Start()
        {
            _positionsesCreator.OnPositionsCreated += _tileInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _characterViewInstantiator.Instantiate;
            _characterCreator.OnCharacterCreated += _deathHandler.Register;
            
            _playerMoney.SetMoney(10);
            _positionsesCreator.Create();
            CreateCharacters();
        }

        private void CreateCharacters()
        {
            _characterCreator.Create(_characterConfigs.First(config => config.Team == Team.Player));
            _characterCreator.Create(_characterConfigs.First(config => config.Team == Team.Bot));
        }
    }
}