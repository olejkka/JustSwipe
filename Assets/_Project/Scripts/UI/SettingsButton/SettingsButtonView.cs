using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        
        public void Initialize(Lifetime lifetime, Action settingsClicked)
        {
            lifetime.BracketButton(_button, () => settingsClicked?.Invoke());
        }
    }
}