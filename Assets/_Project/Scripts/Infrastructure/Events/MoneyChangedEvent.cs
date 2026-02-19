namespace _Project.Scripts.Infrastructure.Events
{
    public class MoneyChangedEvent
    {
        public int Value { get; }
        public MoneyChangedEvent(int value) => Value = value;
    }
}