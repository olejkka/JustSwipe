using JetBrains.Lifetimes;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.Infrastructure.LifetimesExtensions
{
    public static class LifetimeUiExtensions
    {
        public static void BracketButton(this Lifetime lifetime, Button button, UnityAction listener)
        {
            lifetime.Bracket(
                () => button.onClick.AddListener(listener),
                () => button.onClick.RemoveListener(listener));
        }
    }
}