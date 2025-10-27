using System;

namespace _Project.Scripts.Wallet
{
    public class Money
    {
        public int Amount { get; private set; }
        
        public event Action<int> OnAmountChanged;

        
        public void SetAmount(int amount)
        {
            Amount = amount;
            OnAmountChanged?.Invoke(Amount);
        }
        
        public void ChangeAmount(int amount)
        {
            Amount += amount;
            OnAmountChanged?.Invoke(Amount);
        }
    }
}