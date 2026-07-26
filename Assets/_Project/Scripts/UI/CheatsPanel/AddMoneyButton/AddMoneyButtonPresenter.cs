using System;
using _Project.Scripts.GameplayEconomy;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.CheatsPanel.AddMoneyButton
{
    public class AddMoneyButtonPresenter : IStartable, IDisposable
    {
        private const int AmountToAdd = 100;

        private readonly AddMoneyButtonView _view;
        private readonly GameplayMoney _gameplayMoney;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public AddMoneyButtonPresenter(AddMoneyButtonView view, GameplayMoney gameplayMoney)
        {
            _view = view;
            _gameplayMoney = gameplayMoney;
        }

        public void Start() =>
            _view.Initialize(_lifetimeDefinition.Lifetime, OnClicked);

        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnClicked() =>
            _gameplayMoney.ChangeAmount(AmountToAdd);
    }
}