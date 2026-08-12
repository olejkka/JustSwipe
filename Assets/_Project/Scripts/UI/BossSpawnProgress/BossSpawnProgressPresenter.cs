using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.UI.GameplayStatistic;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.BossSpawnProgress
{
    public class BossSpawnProgressPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly BossSpawnProgressView _view;
        private readonly GameplayStatisticsService _gameplayStatisticsService;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public BossSpawnProgressPresenter(
            EventBus eventBus,
            BossSpawnProgressView view,
            GameplayStatisticsService gameplayStatisticsService,
            InitialGameplayConfig initialGameplayConfig)
        {
            _eventBus = eventBus;
            _view = view;
            _gameplayStatisticsService = gameplayStatisticsService;
            _initialGameplayConfig = initialGameplayConfig;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);

            Refresh();
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Killer == null || e.Killer.Team != Team.Player || e.Character.Team == Team.Player)
                return;

            Refresh();
        }

        private void Refresh()
        {
            var killed = _gameplayStatisticsService.EnemiesKilledUntilBoss;
            var threshold = _initialGameplayConfig.BossSpawnThreshold;

            if (threshold > 0 && killed >= threshold)
            {
                _gameplayStatisticsService.ResetEnemiesKilledUntilBoss();
                killed = 0;
            }

            _view.SetProgress(killed, threshold);
        }
    }
}