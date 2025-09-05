using System;
using _Project.Scripts.Economy;
using VContainer.Unity;

namespace _Project.Scripts.UI.Money
{
    public class PlayerMoneyPresenter : IStartable, IDisposable
    {
        private readonly PlayerMoney _playerMoney;
        private readonly PlayerMoneyView _playerMoneyView;

        public PlayerMoneyPresenter(PlayerMoney playerMoney, PlayerMoneyView playerMoneyView)
        {
            _playerMoney = playerMoney;
            _playerMoneyView = playerMoneyView;
        }

        public void Start()
        {
            _playerMoney.OnMoneyChanged += OnMoneyChanged;
            
            _playerMoneyView.UpdateMoneyDisplay(_playerMoney.Money);
        }

        public void Dispose()
        {
            _playerMoney.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int newMoney)
        {
            _playerMoneyView.UpdateMoneyDisplay(newMoney);
        }
    }
}