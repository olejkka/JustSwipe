using System;
using _Project.Scripts.Wallet;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly MoneyView _view;
        private readonly Money _money;

        public MoneyPresenter(MoneyView view, Money money)
        {
            _view = view;
            _money = money;
        }

        public void Start()
        {
            _money.OnAmountChanged += OnMoneyChanged;
            
            OnMoneyChanged(_money.Amount);
        }

        public void Dispose()
        {
            _money.OnAmountChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int newAmount)
        {
            _view.UpdateAmountFormatted(newAmount);
        }
    }
}