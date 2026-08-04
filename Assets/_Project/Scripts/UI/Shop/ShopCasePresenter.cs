using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Effects;
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
        private readonly EffectsConfig _effectsConfig;
        private readonly EffectCaseColorsConfig _colorsConfig;
        private readonly ShopPurchaseService _shopPurchaseService;
        private readonly EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        private ShopOffer _currentOffer;

        
        public ShopCasePresenter(
            ShopCaseView view,
            CharactersConfig charactersConfig,
            EffectsConfig effectsConfig,
            EffectCaseColorsConfig colorsConfig,
            ShopPurchaseService shopPurchaseService,
            EventBus eventBus)
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _effectsConfig = effectsConfig;
            _colorsConfig = colorsConfig;
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
            var offerType = UnityEngine.Random.value < 0.5f
                ? ShopOfferType.Character
                : ShopOfferType.Effect;
            _currentOffer = offerType == ShopOfferType.Character
                ? BuildCharacterOffer()
                : BuildEffectOffer();
            if (_currentOffer == null)
            {
                Debug.LogError("Failed to build shop offer");
                return;
            }
            _view.SetData(_currentOffer);
        }
        
        private ShopOffer BuildCharacterOffer()
        {
            CharacterType? excludedType = null;

            if (_currentOffer != null && _currentOffer.Type == ShopOfferType.Character)
            {
                var previous = _charactersConfig.GetEntryByDefinitionId(_currentOffer.DefinitionId);
                excludedType = previous?.CharacterType;
            }

            var entry = _charactersConfig.GetRandomEntryByTeamExcept(
                            Team.Player,
                            excludedType)
                        ?? _charactersConfig.GetRandomEntryByTeam(
                            Team.Player);

            if (entry == null)
                return null;

            return new ShopOffer
            {
                Type = ShopOfferType.Character,
                DefinitionId = entry.DefinitionId,
                Price = entry.Price,
                Icon = entry.Icon,
                Health = entry.BaseStats.Health,
                Damage = entry.BaseStats.Damage
            };
        }
        
        private ShopOffer BuildEffectOffer()
        {
            var entries = _effectsConfig.EffectEntries.FindAll(
                entry => entry.Polarity != EffectPolarity.None);

            if (entries.Count == 0)
                return null;

            string excludedId = _currentOffer != null &&
                                _currentOffer.Type == ShopOfferType.Effect
                ? _currentOffer.DefinitionId
                : null;

            EffectDefinition selected = null;

            if (!string.IsNullOrEmpty(excludedId))
            {
                var filtered = entries.FindAll(
                    entry => entry.DefinitionId != excludedId);

                if (filtered.Count > 0)
                    selected = filtered[UnityEngine.Random.Range(0, filtered.Count)];
            }

            selected ??= entries[UnityEngine.Random.Range(0, entries.Count)];

            var targetTeam = selected.Polarity switch
            {
                EffectPolarity.Buff => Team.Player,
                EffectPolarity.Debuff => Team.Bot,
                _ => Team.None
            };

            return new ShopOffer
            {
                Type = ShopOfferType.Effect,
                DefinitionId = selected.DefinitionId,
                Price = selected.Price,
                Icon = selected.Icon,
                TargetTeam = targetTeam,
                EffectType = selected.Type,
                EffectParameter = selected.Parameter,
                Turns = selected.Turns,
                BackgroundColor = _colorsConfig.GetBackgroundColor(selected.Polarity)
            };
        }

        private void OnPurchaseClicked()
        {
            if (!_shopPurchaseService.TryPurchase(_currentOffer))
                return;
            
            RefreshCase();
        }

        private void OnRerollClicked(ShopCaseRerollEvent e)
        {
            RefreshCase();
        }
    }
}