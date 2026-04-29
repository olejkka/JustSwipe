using System;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
{
    public class EventTransition<TEvent, TState> : ITransition where TState : IState
    {
        private readonly EventBus.EventBus _eventBus;
        private bool _triggered;
        private bool _active;

        public Type NextState => typeof(TState);

        public EventTransition(EventBus.EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public bool CanTransit() => _triggered;

        private void OnEvent(TEvent e)
        {
            _triggered = true;
        }
        
        public void Activate()
        {
            if (_active)
                return;
            
            _active = true;
            _eventBus.Subscribe<TEvent>(OnEvent);
        }
        
        public void Deactivate()
        {
            if (!_active)
                return;
            
            _active = false;
            _eventBus.Unsubscribe<TEvent>(OnEvent);
        }
        
        public void Reset()
        {
            _triggered = false;
        }
    }
}