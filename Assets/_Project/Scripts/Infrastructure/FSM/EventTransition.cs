using System;
using _Project.Scripts.Infrastructure.Events;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class EventTransition<TEvent, TState> : ITransition where TState : IState
    {
        private readonly EventBus _eventBus;
        private bool _triggered;

        public Type NextState => typeof(TState);

        public EventTransition(EventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<TEvent>(OnEvent);
        }

        public bool CanTransit() => _triggered;

        private void OnEvent(TEvent e)
        {
            _triggered = true;
        }
        
        public void Reset()
        {
            _triggered = false;
        }
    }
}