using System;
using _Project.Scripts.Economy;
using VContainer.Unity;

namespace _Project.Scripts.UI.MoneyUI
{
    public class PlayerMoneyPresenter : IStartable, IDisposable
    {
        private readonly PlayerMoney _playerMoney;
        private readonly PlayerMoneyView _view;

        public PlayerMoneyPresenter(PlayerMoney playerMoney, PlayerMoneyView view)
        {
            _playerMoney = playerMoney;
            _view = view;
        }

        public void Start()
        {
            _playerMoney.OnMoneyChanged += OnMoneyChanged;
            
            _view.UpdateMoneyDisplay(_playerMoney.Money);
        }

        public void Dispose()
        {
            _playerMoney.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int newMoney)
        {
            _view.UpdateMoneyDisplay(newMoney);
        }
    }
}