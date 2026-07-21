using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.ApplyEffectButton
{
    public class ApplyEffectButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        
        public void Initialize(Lifetime lifetime, Action applyEffectClicked)
        {
            lifetime.BracketButton(_button, () => applyEffectClicked?.Invoke());
        }
    }
}