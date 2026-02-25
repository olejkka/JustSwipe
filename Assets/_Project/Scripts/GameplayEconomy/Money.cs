using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;

namespace _Project.Scripts.Wallet
{
    public class Money
    {
        private readonly EventBus _eventBus;
        
        public int Amount { get; private set; }


        public Money(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void SetAmount(int amount)
        {
            Amount = amount;

            _eventBus.Publish(new MoneyChangedEvent(Amount));
        }
        
        public void ChangeAmount(int amount)
        {
            Amount += amount;
            
            _eventBus.Publish(new MoneyChangedEvent(Amount));
        }
    }
}