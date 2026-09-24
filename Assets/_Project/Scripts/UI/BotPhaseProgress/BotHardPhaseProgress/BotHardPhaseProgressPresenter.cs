using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.BotPhaseProgress.BotHardPhaseProgress
{
    public class BotHardPhaseProgressPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly BotHardPhaseProgressView _view;
        private readonly BotPhaseService _botPhaseService;
        private readonly BotPhaseCharactersConfig _botPhaseCharactersConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();


        public BotHardPhaseProgressPresenter(
            EventBus eventBus,
            BotHardPhaseProgressView view,
            BotPhaseService botPhaseService,
            BotPhaseCharactersConfig botPhaseCharactersConfig)
        {
            _eventBus = eventBus;
            _view = view;
            _botPhaseService = botPhaseService;
            _botPhaseCharactersConfig = botPhaseCharactersConfig;
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
            if (e.Character.Team == Team.Player || e.Source.OwnerTeam != Team.Player)
                return;

            Refresh();
        }

        private void Refresh()
        {
            _view.SetProgress(
                _botPhaseService.EnemiesKilledUntilHardPhase,
                _botPhaseCharactersConfig.BotHardPhaseThreshold);
            _view.SetPhaseActive(_botPhaseService.Phase == BotPhase.Hard);
        }
    }
}
