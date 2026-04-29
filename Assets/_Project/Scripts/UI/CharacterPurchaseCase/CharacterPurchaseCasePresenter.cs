using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseCasePresenter : IStartable, IDisposable
    {
        private readonly CharacterPurchaseCaseView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterPurchaseService _characterPurchaseService;
        private readonly EventBus _eventBus;

        private CharacterDefinition _currentEntry;

        public CharacterPurchaseCasePresenter(
            CharacterPurchaseCaseView view,
            CharactersConfig charactersConfig,
            CharacterPurchaseService characterPurchaseService,
            EventBus eventBus
            )
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _characterPurchaseService = characterPurchaseService;
            _eventBus = eventBus;
        }

        public void Start()
        {
            _view.OnPurchaseClicked += OnPurchaseClicked;
            _eventBus.Subscribe<CharacterPurchaseCaseRerollEvent>(OnRerollClicked);
            
            RefreshCase();
        }

        public void Dispose()
        {
            _view.OnPurchaseClicked -= OnPurchaseClicked;
            _eventBus.Unsubscribe<CharacterPurchaseCaseRerollEvent>(OnRerollClicked);
        }

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
            if (!_characterPurchaseService.TryPurchase(_currentEntry.DefinitionId, _currentEntry.Price))
                return;

            RefreshCase();
        }

        private void OnRerollClicked(CharacterPurchaseCaseRerollEvent e)
        {
            RefreshCase();
        }
    }
}