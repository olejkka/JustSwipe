using System;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCasesContainerPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharacterCasesContainerView _characterCasesContainerView;
        private readonly CharacterCaseUIView[] _caseViews;
        private readonly CharacterCaseUIPresenter[] _casePresenters;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersConfig _charactersConfig;

        private bool _initialized;

        public CharacterCasesContainerPresenter(
            EventBus eventBus,
            CharacterCasesContainerView containerView,
            InitialGameplayConfig config,
            CharactersStorage charactersStorage,
            CharactersConfig charactersConfig)
        {
            _eventBus = eventBus;
            _caseViews = containerView.CreateCases(config.MaxPlayerCharactersCount);
            _charactersStorage = charactersStorage;
            _charactersConfig = charactersConfig;

            _casePresenters = new CharacterCaseUIPresenter[_caseViews.Length];
        }

        public void Start()
        {
            _eventBus.Subscribe<CharacterCreatedEvent>(OnCharacterCreated);
            _eventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);

            EnsureInitialized();
            SyncExistingCharacters();
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<CharacterCreatedEvent>(OnCharacterCreated);
            _eventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);

            for (int i = 0; i < _casePresenters.Length; i++)
            {
                _casePresenters[i]?.Dispose();
                _casePresenters[i] = null;
            }
        }

        private void OnCharacterCreated(CharacterCreatedEvent e)
        {
            if (e.Character.Team != Team.Player) return;

            for (int i = 0; i < _casePresenters.Length; i++)
            {
                if (!_casePresenters[i].IsAssigned())
                {
                    _casePresenters[i].AssignCharacter(e.Character);
                    return;
                }
            }
        }

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            for (int i = 0; i < _casePresenters.Length; i++)
            {
                if (_casePresenters[i].IsAssignedTo(e.Character))
                {
                    _casePresenters[i].UnassignCharacter();
                    return;
                }
            }
        }

        private void SyncExistingCharacters()
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(Team.Player))
            {
                for (int i = 0; i < _casePresenters.Length; i++)
                {
                    if (!_casePresenters[i].IsAssigned())
                    {
                        _casePresenters[i].AssignCharacter(character);
                        break;
                    }
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            for (int i = 0; i < _caseViews.Length; i++)
            {
                _casePresenters[i] = new CharacterCaseUIPresenter(_caseViews[i], _charactersStorage, _charactersConfig);
                _casePresenters[i].Start();
            }

            _initialized = true;
        }
    }
}