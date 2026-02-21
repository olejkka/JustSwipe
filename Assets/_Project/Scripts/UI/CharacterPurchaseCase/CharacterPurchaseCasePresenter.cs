using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Wallet;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseCasePresenter : IStartable, IDisposable
    {
        private readonly CharacterPurchaseCaseView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterPurchaseService _characterPurchaseService;

        private CharactersConfig.CharacterEntry _currentEntry;

        public CharacterPurchaseCasePresenter(
            CharacterPurchaseCaseView view,
            CharactersConfig charactersConfig,
            CharacterPurchaseService characterPurchaseService
            )
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _characterPurchaseService = characterPurchaseService;
        }

        public void Start()
        {
            _view.OnPurchaseClicked += OnPurchaseClicked;

            RefreshCase();
        }

        public void Dispose()
        {
            _view.OnPurchaseClicked -= OnPurchaseClicked;
        }

        private void RefreshCase()
        {
            _currentEntry = _charactersConfig.GetRandomEntryByTeam(Team.Player);

            if (_currentEntry == null)
            {
                Debug.LogError("No player characters found in config!");
                return;
            }

            _view.SetData(
                _currentEntry.Sprite,
                _currentEntry.Price,
                _currentEntry.CharacterBaseStats.Health,
                _currentEntry.CharacterBaseStats.Damage
            );
        }

        private void OnPurchaseClicked()
        {
            if (!_characterPurchaseService.TryPurchase(_currentEntry.CharacterType, _currentEntry.Price))
                return;

            RefreshCase();
        }
    }
}