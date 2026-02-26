using System;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.MoneyUI
{
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly MoneyView _view;
        private readonly GameplayMoney _gameplayMoney;

        public MoneyPresenter(EventBus eventBus, MoneyView view, GameplayMoney gameplayMoney)
        {
            _eventBus = eventBus;
            _view = view;
            _gameplayMoney = gameplayMoney;
        }

        public void Start()
        {
            _eventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
            _view.UpdateAmountFormatted(_gameplayMoney.Amount);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<MoneyChangedEvent>(OnMoneyChanged);
        }

        private void OnMoneyChanged(MoneyChangedEvent e)
        {
            _view.UpdateAmountFormatted(e.Value);
        }
    }
}