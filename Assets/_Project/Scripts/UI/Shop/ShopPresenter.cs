using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Effects;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.Shop
{
    public class ShopPresenter : IStartable, IDisposable
    {
        private readonly ShopView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly EffectsConfig _effectsConfig;
        private readonly EffectCaseColorsConfig _colorsConfig;
        private readonly ShopPurchaseService _shopPurchaseService;
        private readonly RerollPurchaseService _rerollPurchaseService;
        private readonly GameplayEconomyConfig _gameplayEconomyConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private ShopOffer _characterOffer;
        private ShopOffer _effectOffer;

        
        public ShopPresenter(
            ShopView view,
            CharactersConfig charactersConfig,
            EffectsConfig effectsConfig,
            EffectCaseColorsConfig colorsConfig,
            ShopPurchaseService shopPurchaseService,
            RerollPurchaseService rerollPurchaseService,
            GameplayEconomyConfig gameplayEconomyConfig)
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _effectsConfig = effectsConfig;
            _colorsConfig = colorsConfig;
            _shopPurchaseService = shopPurchaseService;
            _rerollPurchaseService = rerollPurchaseService;
            _gameplayEconomyConfig = gameplayEconomyConfig;
        }

        public void Start()
        {
            _view.Initialize(
                _lifetimeDefinition.Lifetime,
                OnCharacterPurchaseClicked,
                OnCharacterRerollClicked,
                OnEffectPurchaseClicked,
                OnEffectRerollClicked);

            _view.SetRerollPrice(_gameplayEconomyConfig.RerollPrice);

            RefreshCharacterCase();
            RefreshEffectCase();
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnCharacterPurchaseClicked()
        {
            if (!_shopPurchaseService.TryPurchase(_characterOffer))
                return;

            RefreshCharacterCase();
        }

        private void OnEffectPurchaseClicked()
        {
            if (!_shopPurchaseService.TryPurchase(_effectOffer))
                return;

            RefreshEffectCase();
        }

        private void OnCharacterRerollClicked()
        {
            if (!_rerollPurchaseService.TryPurchase(_gameplayEconomyConfig.RerollPrice))
            {
                _view.PlayCharacterRerollFail();
                return;
            }

            _view.PlayCharacterRerollSuccess();
            RefreshCharacterCase();
        }

        private void OnEffectRerollClicked()
        {
            if (!_rerollPurchaseService.TryPurchase(_gameplayEconomyConfig.RerollPrice))
            {
                _view.PlayEffectRerollFail();
                return;
            }

            _view.PlayEffectRerollSuccess();
            RefreshEffectCase();
        }

        private void RefreshCharacterCase()
        {
            _characterOffer = BuildCharacterOffer();
            if (_characterOffer == null)
            {
                Debug.LogError("Failed to build character shop offer");
                return;
            }

            _view.SetCharacterOffer(_characterOffer);
        }

        private void RefreshEffectCase()
        {
            _effectOffer = BuildEffectOffer();
            if (_effectOffer == null)
            {
                Debug.LogError("Failed to build effect shop offer");
                return;
            }

            _view.SetEffectOffer(_effectOffer);
        }

        private ShopOffer BuildCharacterOffer()
        {
            CharacterType? excludedType = null;

            if (_characterOffer != null)
            {
                var previous = _charactersConfig.GetEntryByDefinitionId(_characterOffer.DefinitionId);
                excludedType = previous?.CharacterType;
            }

            var entry = _charactersConfig.GetRandomEntryByTeamExcept(Team.Player, excludedType)
                        ?? _charactersConfig.GetRandomEntryByTeam(Team.Player);

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

            string excludedId = _effectOffer?.DefinitionId;

            EffectDefinition selected = null;

            if (!string.IsNullOrEmpty(excludedId))
            {
                var filtered = entries.FindAll(entry => entry.DefinitionId != excludedId);
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
    }
}