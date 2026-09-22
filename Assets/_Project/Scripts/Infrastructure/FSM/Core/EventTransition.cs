using System;
using _Project.Scripts.Infrastructure.FSM.GameplaySM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;

namespace _Project.Scripts.Infrastructure.FSM.Core
{
    public class EventTransition<TEvent, TState> : ITransition where TState : IState
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly Func<bool> _condition;
        private bool _triggered;
        private bool _active;

        public Type NextState => typeof(TState);

        public EventTransition(EventBus.EventBus eventBus, Func<bool> condition = null)
        {
            _eventBus = eventBus;
            _condition = condition;
        }

        public bool CanTransit() => _triggered && (_condition == null || _condition());

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