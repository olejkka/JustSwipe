using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Wallet;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class CharacterPurchaseCasePresenter : IStartable, IDisposable
    {
        private readonly CharacterPurchaseCaseView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterCreator _characterCreator;
        private readonly Money _money;

        private CharactersConfig.CharacterEntry _currentEntry;

        public CharacterPurchaseCasePresenter(
            CharacterPurchaseCaseView view,
            CharactersConfig charactersConfig,
            CharacterCreator characterCreator,
            Money money)
        {
            _view = view;
            _charactersConfig = charactersConfig;
            _characterCreator = characterCreator;
            _money = money;
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
            if (_currentEntry == null || _money == null)
                return;

            if (_money.Amount < _currentEntry.Price)
            {
                Debug.Log("Not enough money!");
                return;
            }

            _money.ChangeAmount(-_currentEntry.Price);
            _characterCreator.CreateOnRandomPos(_currentEntry.CharacterType);

            RefreshCase();
        }
    }
}