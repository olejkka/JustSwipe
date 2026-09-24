using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.GameplayEconomy
{
    public class KillRewardHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly GameplayMoney _gameplayMoney;
        private readonly CharactersConfig _charactersConfig;
        private readonly BotPhaseCharactersConfig _botPhaseCharactersConfig;
        private readonly GameplayStatisticsService _gameplayStatisticsService;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        

        public KillRewardHandler(
            EventBus eventBus,
            GameplayMoney gameplayMoney, 
            CharactersConfig charactersConfig,
            BotPhaseCharactersConfig botPhaseCharactersConfig,
            GameplayStatisticsService gameplayStatisticsService)
        {
            _eventBus = eventBus;
            _gameplayMoney = gameplayMoney;
            _charactersConfig = charactersConfig;
            _botPhaseCharactersConfig = botPhaseCharactersConfig;
            _gameplayStatisticsService = gameplayStatisticsService;
        }

        public void Start() =>
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);
        
        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Character.Team == Team.Player || e.Source.OwnerTeam != Team.Player)
                return;

            var definitionId = e.Character.DefinitionId;
            var listedPhase = _botPhaseCharactersConfig.RequireListedPhase(definitionId);
            var entry = _charactersConfig.GetEntryByDefinitionId(definitionId);

            _gameplayMoney.ChangeAmount(entry.Reward);

            switch (listedPhase)
            {
                case BotPhase.Default:
                    _gameplayStatisticsService.AddDefaultEnemyKill(entry.Reward);
                    break;
                case BotPhase.Hard:
                    _gameplayStatisticsService.AddHardEnemyKill(entry.Reward);
                    break;
                case BotPhase.Boss:
                    _gameplayStatisticsService.AddBossEnemyKill(entry.Reward);
                    break;
            }
        }
    }
}