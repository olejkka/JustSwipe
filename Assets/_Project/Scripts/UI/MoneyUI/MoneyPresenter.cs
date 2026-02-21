using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Wallet;
using VContainer.Unity;

namespace _Project.Scripts.UI.MoneyUI
{
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly MoneyView _view;
        private readonly Money _money;

        public MoneyPresenter(EventBus eventBus, MoneyView view, Money money)
        {
            _eventBus = eventBus;
            _view = view;
            _money = money;
        }

        public void Start()
        {
            _eventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
            _view.UpdateAmountFormatted(_money.Amount);
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