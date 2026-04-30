using System;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.MoneyUI
{
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly MoneyView _view;
        private readonly GameplayMoney _gameplayMoney;

        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        
        public MoneyPresenter(EventBus eventBus, MoneyView view, GameplayMoney gameplayMoney)
        {
            _eventBus = eventBus;
            _view = view;
            _gameplayMoney = gameplayMoney;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<MoneyChangedEvent>(_lifetimeDefinition.Lifetime, OnMoneyChanged);
            
            _view.UpdateAmount(_gameplayMoney.Amount);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        private void OnMoneyChanged(MoneyChangedEvent e)
        {
            _view.UpdateAmount(e.Value);
        }
    }
}