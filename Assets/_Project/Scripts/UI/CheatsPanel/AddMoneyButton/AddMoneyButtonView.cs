using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CheatsPanel.AddMoneyButton
{
    public class AddMoneyButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        
        public void Initialize(Lifetime lifetime, Action clicked)
        {
            lifetime.BracketButton(_button, () => clicked?.Invoke());
        }
    }
}