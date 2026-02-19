using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.MoneyUI
{
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly MoneyView _view;

        public MoneyPresenter(EventBus eventBus, MoneyView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start()
        {
            _eventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
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