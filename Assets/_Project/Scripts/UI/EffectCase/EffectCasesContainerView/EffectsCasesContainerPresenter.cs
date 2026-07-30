using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.StructsEnums;
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
        private readonly EffectsConfig _effectsConfig;
        private readonly EffectCaseColorsConfig _colorsConfig;

        private readonly EffectCaseUIView[] _playerCaseViews;
        private readonly EffectCaseUIView[] _botCaseViews;
        private readonly EffectCaseUIPresenter[] _playerCasePresenters;
        private readonly EffectCaseUIPresenter[] _botCasePresenters;

        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private bool _initialized;


        public EffectsCasesContainerPresenter(
            EventBus eventBus,
            CharactersStorage charactersStorage,
            CharactersViewsStorage charactersViewsStorage,
            EffectsConfig effectsConfig,
            EffectCaseColorsConfig colorsConfig,
            EffectsCasesContainerView containerView,
            InitialGameplayConfig initialGameplayConfig)
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
            _effectsConfig = effectsConfig;
            _colorsConfig = colorsConfig;

            var casesCount = Math.Max(
                1,
                initialGameplayConfig.MaxEffectsCount);

            _playerCaseViews = containerView.CreatePlayerCases(casesCount);
            _botCaseViews = containerView.CreateBotCases(casesCount);

            _playerCasePresenters =
                new EffectCaseUIPresenter[_playerCaseViews.Length];

            _botCasePresenters =
                new EffectCaseUIPresenter[_botCaseViews.Length];
        }

        public void Start()
        {
            EnsureInitialized();

            _eventBus.SubscribeWithLifetime<ApplyEffectEvent>(
                _lifetimeDefinition.Lifetime,
                OnApplyEffect);

            _eventBus.SubscribeWithLifetime<TurnEndedEvent>(
                _lifetimeDefinition.Lifetime,
                OnTurnEnded);

            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);

            SyncEffectCases(Team.Player);
            SyncEffectCases(Team.Bot);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();

            DisposePresenters(_playerCasePresenters);
            DisposePresenters(_botCasePresenters);
        }

        private void OnApplyEffect(ApplyEffectEvent e) =>
            SyncEffectCases(e.Team);

        private void OnTurnEnded(TurnEndedEvent e) =>
            SyncEffectCases(e.Team);

        private void OnCharacterDied(CharacterDiedEvent e) =>
            SyncEffectCases(e.Character.Team);

        private void SyncEffectCases(Team team)
        {
            EnsureInitialized();

            var presenters = GetPresenters(team);
            var activeEffects = CollectActiveEffects(team);

            foreach (var presenter in presenters)
                presenter.UnassignEffect();

            var index = 0;

            foreach (var effect in activeEffects.Values)
            {
                if (index >= presenters.Length)
                    break;

                presenters[index].AssignEffect(effect);
                index++;
            }
        }

        private Dictionary<int, Effect> CollectActiveEffects(Team team)
        {
            var result = new Dictionary<int, Effect>();

            foreach (var character in
                     _charactersStorage.GetCharactersByTeam(team))
            {
                foreach (var effect in character.Effects)
                {
                    if (result.TryGetValue(
                            effect.InstanceId,
                            out var existing))
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

            InitializePresenters(
                _playerCaseViews,
                _playerCasePresenters,
                Team.Player);

            InitializePresenters(
                _botCaseViews,
                _botCasePresenters,
                Team.Bot);

            _initialized = true;
        }

        private void InitializePresenters(
            EffectCaseUIView[] views,
            EffectCaseUIPresenter[] presenters,
            Team team)
        {
            for (var i = 0; i < views.Length; i++)
            {
                presenters[i] = new EffectCaseUIPresenter(
                    _lifetimeDefinition.Lifetime,
                    views[i],
                    _effectsConfig,
                    _colorsConfig,
                    _charactersStorage,
                    _charactersViewsStorage,
                    team);

                presenters[i].Start();
            }
        }

        private EffectCaseUIPresenter[] GetPresenters(Team team)
        {
            return team switch
            {
                Team.Player => _playerCasePresenters,
                Team.Bot => _botCasePresenters,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(team),
                    team,
                    null)
            };
        }

        private static void DisposePresenters(
            EffectCaseUIPresenter[] presenters)
        {
            for (var i = 0; i < presenters.Length; i++)
            {
                presenters[i]?.Dispose();
                presenters[i] = null;
            }
        }
    }
}