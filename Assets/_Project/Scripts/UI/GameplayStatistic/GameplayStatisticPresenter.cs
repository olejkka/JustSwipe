using System;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.GameplayStatistic
{
    public class GameplayStatisticPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly GameplayStatisticView _view;
        private readonly GameplayStatisticsService _gameplayStatisticsService;
        private readonly LifetimeDefinition _lifetimeDefinition = new();


        public GameplayStatisticPresenter(EventBus eventBus, GameplayStatisticView view, GameplayStatisticsService gameplayStatisticsService)
        {
            _eventBus = eventBus;
            _view = view;
            _gameplayStatisticsService = gameplayStatisticsService;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<ShowGameplayStatisticEvent>(_lifetimeDefinition.Lifetime, OnShowGameplayStatisticEvent);
            
            _view.Initialize(_lifetimeDefinition.Lifetime, ApplicationQuitClicked);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        private void OnShowGameplayStatisticEvent(ShowGameplayStatisticEvent _)
        {
            _gameplayStatisticsService.GetSnapshot();
            
            _view.SetContainerActive(true);
            _view.SetTurnsCount(_gameplayStatisticsService.GetSnapshot().TurnsCount.ToString());
            _view.SetCountEnemiesKilled(_gameplayStatisticsService.GetSnapshot().EnemiesKilled.ToString());
            _view.SetGoldEarned(_gameplayStatisticsService.GetSnapshot().GoldEarned.ToString());
        }

        private void ApplicationQuitClicked()
        {
            _eventBus.Publish(new ApplicationQuitRequestedEvent());
        }
    }
}