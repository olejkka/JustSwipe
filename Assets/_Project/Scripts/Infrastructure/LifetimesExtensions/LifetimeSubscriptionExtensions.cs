using System;
using JetBrains.Lifetimes;

namespace _Project.Scripts.Infrastructure.LifetimesExtensions
{
    public static class LifetimeSubscriptionExtensions
    {
        public static void BracketSubscription(this Lifetime lifetime, Action subscribe, Action unsubscribe)
        {
            lifetime.Bracket(subscribe, unsubscribe);
        }
    }
}