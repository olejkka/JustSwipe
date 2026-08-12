using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCasesContainerPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterCaseColorsConfig _colorsConfig;

        private readonly CharacterCaseUIView[] _playerCaseViews;
        private readonly CharacterCaseUIView[] _botCaseViews;
        private readonly CharacterCaseUIPresenter[] _playerCasePresenters;
        private readonly CharacterCaseUIPresenter[] _botCasePresenters;

        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private bool _initialized;

        
        public CharacterCasesContainerPresenter(
            EventBus eventBus,
            CharactersViewsStorage charactersViewsStorage,
            CharacterCasesContainerView containerView,
            InitialGameplayConfig config,
            CharactersStorage charactersStorage,
            CharactersConfig charactersConfig,
            CharacterCaseColorsConfig colorsConfig)
        {
            _eventBus = eventBus;
            _charactersViewsStorage = charactersViewsStorage;
            _charactersStorage = charactersStorage;
            _charactersConfig = charactersConfig;
            _colorsConfig = colorsConfig;

            var casesCount = Math.Max(1, config.MaxPlayerCharactersCount);

            _playerCaseViews = containerView.CreatePlayerCases(casesCount);
            _botCaseViews = containerView.CreateBotCases(casesCount);

            _playerCasePresenters =
                new CharacterCaseUIPresenter[_playerCaseViews.Length];

            _botCasePresenters =
                new CharacterCaseUIPresenter[_botCaseViews.Length];
        }

        public void Start()
        {
            EnsureInitialized();
            
            _eventBus.SubscribeWithLifetime<CharacterCreatedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterCreated);
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);
            
            SyncExistingCharacters(Team.Player);
            SyncExistingCharacters(Team.Bot);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();

            DisposePresenters(_playerCasePresenters);
            DisposePresenters(_botCasePresenters);
        }

        private void OnCharacterCreated(CharacterCreatedEvent e)
        {
            var presenters = GetPresenters(e.Character.Team);

            for (var i = 0; i < presenters.Length; i++)
            {
                if (!presenters[i].IsAssigned())
                {
                    presenters[i].AssignCharacter(e.Character);
                    return;
                }
            }
        }

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            var presenters = GetPresenters(e.Character.Team);

            for (var i = 0; i < presenters.Length; i++)
            {
                if (presenters[i].IsAssignedTo(e.Character))
                {
                    presenters[i].UnassignCharacter();
                    return;
                }
            }
        }

        private void SyncExistingCharacters(Team team)
        {
            var presenters = GetPresenters(team);

            foreach (var character in _charactersStorage.GetCharactersByTeam(team))
            {
                for (var i = 0; i < presenters.Length; i++)
                {
                    if (!presenters[i].IsAssigned())
                    {
                        presenters[i].AssignCharacter(character);
                        break;
                    }
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            InitializePresenters(_playerCaseViews, _playerCasePresenters);
            InitializePresenters(_botCaseViews, _botCasePresenters);

            _initialized = true;
        }

        private void InitializePresenters(
            CharacterCaseUIView[] views,
            CharacterCaseUIPresenter[] presenters)
        {
            for (var i = 0; i < views.Length; i++)
            {
                presenters[i] = new CharacterCaseUIPresenter(
                    _lifetimeDefinition.Lifetime,
                    views[i],
                    _charactersConfig,
                    _charactersViewsStorage,
                    _colorsConfig);

                presenters[i].Start();
            }
        }

        private CharacterCaseUIPresenter[] GetPresenters(Team team)
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
            CharacterCaseUIPresenter[] presenters)
        {
            for (var i = 0; i < presenters.Length; i++)
            {
                presenters[i]?.Dispose();
                presenters[i] = null;
            }
        }
    }
}