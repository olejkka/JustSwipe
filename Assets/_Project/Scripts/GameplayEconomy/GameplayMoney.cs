using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.GameplayEconomy
{
    public class GameplayMoney
    {
        private readonly EventBus _eventBus;
        
        public int Amount { get; private set; }


        public GameplayMoney(EventBus eventBus)
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