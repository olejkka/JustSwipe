using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.EffectCase.EffectCasesContainerView
{
    public class EffectsCasesContainerPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly CharacterEffectsConfig _characterEffectsConfig;
        private readonly EffectCaseUIView[] _caseViews;
        private readonly EffectCaseUIPresenter[] _casePresenters;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private bool _initialized;

        
        public EffectsCasesContainerPresenter(
            EventBus eventBus,
            CharactersStorage charactersStorage,
            CharactersViewsStorage charactersViewsStorage,
            CharacterEffectsConfig characterEffectsConfig,
            EffectsCasesContainerView containerView,
            InitialGameplayConfig initialGameplayConfig)
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
            _characterEffectsConfig = characterEffectsConfig;
            
            _caseViews = containerView.CreateCases(Math.Max(1, initialGameplayConfig.MaxEffectsCount));
            _casePresenters = new EffectCaseUIPresenter[_caseViews.Length];
        }

        public void Start()
        {
            EnsureInitialized();

            _eventBus.SubscribeWithLifetime<ApplyEffectEvent>(_lifetimeDefinition.Lifetime, OnApplyEffect);
            _eventBus.SubscribeWithLifetime<TurnEndedEvent>(_lifetimeDefinition.Lifetime, OnTurnEnded);
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);

            SyncBuffCases();
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();

            for (var i = 0; i < _casePresenters.Length; i++)
            {
                _casePresenters[i]?.Dispose();
                _casePresenters[i] = null;
            }
        }

        private void OnApplyEffect(ApplyEffectEvent e)
        {
            if (e.Team != Team.Player)
                return;

            SyncBuffCases();
        }

        private void OnTurnEnded(TurnEndedEvent e)
        {
            if (e.Team != Team.Player)
                return;

            SyncBuffCases();
        }

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Character.Team != Team.Player)
                return;

            SyncBuffCases();
        }

        private void SyncBuffCases()
        {
            EnsureInitialized();

            var activeEffects = CollectActiveEffects();

            foreach (var presenter in _casePresenters)
                presenter.UnassignEffect();

            var index = 0;
            foreach (var effect in activeEffects.Values)
            {
                if (index >= _casePresenters.Length)
                    break;

                _casePresenters[index].AssignEffect(effect);
                index++;
            }
        }

        private Dictionary<int, CharacterEffect> CollectActiveEffects()
        {
            var result = new Dictionary<int, CharacterEffect>();

            foreach (var character in _charactersStorage.GetCharactersByTeam(Team.Player))
            {
                foreach (var effect in character.Effects)
                {
                    if (result.TryGetValue(effect.InstanceId, out var existing))
                    {
                        if (effect.RemainingTurns > existing.RemainingTurns)
                            result[effect.InstanceId] = effect;
                    }
                    else
                    {
                        result.Add(effect.InstanceId, effect);
                    }
                }
            }

            return result;
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            for (var i = 0; i < _caseViews.Length; i++)
            {
                _casePresenters[i] = new EffectCaseUIPresenter(
                    _lifetimeDefinition.Lifetime,
                    _caseViews[i],
                    _characterEffectsConfig,
                    _charactersStorage,
                    _charactersViewsStorage);

                _casePresenters[i].Start();
            }

            _initialized = true;
        }
    }
}