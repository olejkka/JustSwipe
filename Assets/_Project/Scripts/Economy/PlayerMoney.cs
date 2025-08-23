using System;

namespace _Project.Scripts.Economy
{
    public class PlayerMoney
    {
        public event Action<int> OnMoneyChanged;
        
        public int Money { get; private set; }


        public void SetMoney(int money)
        {
            Money = money;
            OnMoneyChanged?.Invoke(Money);
        }

        public void ChangeMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
        }
    
        public bool TrySpendMoney(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                OnMoneyChanged?.Invoke(Money);
                return true;
            }
            
            return false;
        }
    }
}