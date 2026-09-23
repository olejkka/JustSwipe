using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.BotHardPhaseProgress
{
    public class BotHardPhaseProgressPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly BotHardPhaseProgressView _view;
        private readonly GameplayStatisticsService _gameplayStatisticsService;
        private readonly BotPhaseCharactersConfig _botPhaseCharactersConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public BotHardPhaseProgressPresenter(
            EventBus eventBus,
            BotHardPhaseProgressView view,
            GameplayStatisticsService gameplayStatisticsService,
            BotPhaseCharactersConfig botPhaseCharactersConfig)
        {
            _eventBus = eventBus;
            _view = view;
            _gameplayStatisticsService = gameplayStatisticsService;
            _botPhaseCharactersConfig = botPhaseCharactersConfig;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);

            _eventBus.SubscribeWithLifetime<BotHardPhaseEndedEvent>(
                _lifetimeDefinition.Lifetime,
                OnBotHardPhaseEnded);

            Refresh();
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Character.Team == Team.Player || e.Source.OwnerTeam != Team.Player)
                return;

            Refresh();
        }

        private void OnBotHardPhaseEnded(BotHardPhaseEndedEvent e) =>
            _gameplayStatisticsService.UnfreezeEnemiesKilledUntilBoss();

        private void Refresh()
        {
            var killed = _gameplayStatisticsService.EnemiesKilledUntilBoss;
            var threshold = _botPhaseCharactersConfig.BotHardPhaseThreshold;

            if (threshold > 0 && killed >= threshold)
            {
                _gameplayStatisticsService.ResetEnemiesKilledUntilBoss();
                _gameplayStatisticsService.FreezeEnemiesKilledUntilBoss();
                killed = 0;
                _eventBus.Publish(new BossSpawnThresholdReachedEvent());
            }

            _view.SetProgress(killed, threshold);
        }
    }
}