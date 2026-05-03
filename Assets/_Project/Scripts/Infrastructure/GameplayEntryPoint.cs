using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.GameplaySM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.UI.GameplayStatistic;
using JetBrains.Lifetimes;
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
        private readonly EventBus.EventBus _eventBus;
        private readonly GameplayStatisticsService _gameplayStatisticsService;
        
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        

        public GameplayEntryPoint(
            GameplayStateMachine stateMachine,
            InitialGameplayConfig initialGameplayConfig,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            GameplayMoney gameplayMoney,
            EventBus.EventBus eventBus,
            GameplayStatisticsService gameplayStatisticsService)
        {
            _stateMachine = stateMachine;
            _initialGameplayConfig = initialGameplayConfig;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _gameplayMoney = gameplayMoney;
            _eventBus = eventBus;
            _gameplayStatisticsService = gameplayStatisticsService;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<StartGameplayEvent>(_lifetimeDefinition.Lifetime, OnStartGameplay);
        }

        public void Dispose()
        {
            _stateMachine.Stop();
            _lifetimeDefinition.Terminate();
        }
        
        public void OnStartGameplay(StartGameplayEvent e)
        {
            _gameplayStatisticsService.Reset();
            _positionsCreator.Create();
            _gameplayMoney.SetAmount(_initialGameplayConfig.MoneyCount);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.PlayerCharacter);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.BotCharacter);

            _stateMachine.EnterState<PlayerTurnState>();
        }
    }
}