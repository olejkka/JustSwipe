using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM.GameplaySM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayEntryPoint : IStartable, IDisposable
    {
        private readonly GameplayStateMachine _stateMachine;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsCreator;
        private readonly GameplayMoney _gameplayMoney;
        private readonly EventBus _eventBus;
        

        public GameplayEntryPoint(
            GameplayStateMachine stateMachine,
            InitialGameplayConfig initialGameplayConfig,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            GameplayMoney gameplayMoney,
            EventBus eventBus
        )
        {
            _stateMachine = stateMachine;
            _initialGameplayConfig = initialGameplayConfig;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _gameplayMoney = gameplayMoney;
            _eventBus = eventBus;
        }

        public void Start()
        {
            _eventBus.Subscribe<StartGameplayEvent>(OnStartGameplay);
        }

        public void Dispose()
        {
            _stateMachine.Stop();
            _eventBus.Unsubscribe<StartGameplayEvent>(OnStartGameplay);
        }
        
        public void OnStartGameplay(StartGameplayEvent e)
        {
            _positionsCreator.Create();
            _gameplayMoney.SetAmount(_initialGameplayConfig.MoneyCount);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.PlayerCharacter);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.BotCharacter);

            _stateMachine.EnterState<PlayerTurnState>();
        }
    }
}