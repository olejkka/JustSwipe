using System;
using JetBrains.Lifetimes;

namespace _Project.Scripts.Infrastructure.LifetimesExtensions
{
    public static class EventBusLifetimeExtensions
    {
        public static void SubscribeWithLifetime<TEvent>(
            this EventBus.EventBus eventBus,
            Lifetime lifetime,
            Action<TEvent> handler)
        {
            lifetime.Bracket(
                () => eventBus.Subscribe(handler),
                () => eventBus.Unsubscribe(handler));
        }
    }
}