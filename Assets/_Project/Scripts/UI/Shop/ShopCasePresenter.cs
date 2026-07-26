using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.Shop
{
    public class ShopCasePresenter : IStartable, IDisposable
    {
        private readonly ShopCaseView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterEffectsConfig _characterEffectsConfig;
        private readonly ShopPurchaseService _shopPurchaseService;
        private readonly EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        private CharacterDefinition _currentEntry;

        
        public ShopCasePresenter(
            ShopCaseView view,
            CharactersConfig charactersConfig,
            CharacterEffectsConfig characterEffectsConfig,
            ShopPurchaseService shopPurchaseService,
            EventBus eventBus)
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _characterEffectsConfig = characterEffectsConfig;
            _shopPurchaseService = shopPurchaseService;
            _eventBus = eventBus;
        }

        public void Start()
        {
            _view.Initialize(_lifetimeDefinition.Lifetime, OnPurchaseClicked);
            
            _eventBus.SubscribeWithLifetime<ShopCaseRerollEvent>(
                _lifetimeDefinition.Lifetime,
                OnRerollClicked);
            
            RefreshCase();
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void RefreshCase()
        {
            var excludedType = _currentEntry?.CharacterType;
            var nextEntry = _charactersConfig.GetRandomEntryByTeamExcept(Team.Player, excludedType);
            _currentEntry = nextEntry ?? _charactersConfig.GetRandomEntryByTeam(Team.Player);
            
            if (_currentEntry == null)
            {
                Debug.LogError("No player characters found in config!");
                return;
            }

            _view.SetData(
                _currentEntry.Icon,
                _currentEntry.Price,
                _currentEntry.BaseStats.Health,
                _currentEntry.BaseStats.Damage
            );
        }

        private void OnPurchaseClicked()
        {
            if (!_shopPurchaseService.TryPurchase(_currentEntry.DefinitionId, _currentEntry.Price))
                return;

            RefreshCase();
        }

        private void OnRerollClicked(ShopCaseRerollEvent e)
        {
            RefreshCase();
        }
    }
}