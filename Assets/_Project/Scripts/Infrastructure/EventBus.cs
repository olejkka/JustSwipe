using System;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            
            if (_handlers.TryGetValue(type, out var existingDelegate))
            {
                _handlers[type] = Delegate.Combine(existingDelegate, handler);
            }
            else
            {
                _handlers[type] = handler;
            }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            
            if (_handlers.TryGetValue(type, out var existingDelegate))
            {
                var newDelegate = Delegate.Remove(existingDelegate, handler);
                
                if (newDelegate == null)
                    _handlers.Remove(type);
                else
                    _handlers[type] = newDelegate;
            }
        }

        public void Publish<T>(T eventData)
        {
            var type = typeof(T);
            
            if (_handlers.TryGetValue(type, out var handler)) 
                (handler as Action<T>)?.Invoke(eventData);
        }
    }
}