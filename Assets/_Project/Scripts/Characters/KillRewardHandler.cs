using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.Characters
{
    public class KillRewardHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly GameplayMoney _gameplayMoney;
        private readonly CharactersConfig _charactersConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        

        public KillRewardHandler(EventBus eventBus, GameplayMoney gameplayMoney, CharactersConfig charactersConfig)
        {
            _eventBus = eventBus;
            _gameplayMoney = gameplayMoney;
            _charactersConfig = charactersConfig;
        }

        public void Start() =>
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);
        
        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Killer == null || e.Killer.Team != Team.Player || e.Character.Team == Team.Player)
                return;

            var entry = _charactersConfig.GetEntryByDefinitionId(e.Character.DefinitionId);
            _gameplayMoney.ChangeAmount(entry.Reward);
        }
    }
}