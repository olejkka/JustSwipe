using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CheatsPanel.SetTimescale
{
    public class SetTimescalePanelView : MonoBehaviour
    {
        [SerializeField] private Button _button025;
        [SerializeField] private Button _button05;
        [SerializeField] private Button _button1;
        [SerializeField] private Button _button15;
        [SerializeField] private Button _button2;

        
        public void Initialize(
            Lifetime lifetime,
            Action timescale025Clicked,
            Action timescale05Clicked,
            Action timescale1Clicked,
            Action timescale15Clicked,
            Action timescale2Clicked)
        {
            lifetime.BracketButton(_button025, () => timescale025Clicked?.Invoke());
            lifetime.BracketButton(_button05, () => timescale05Clicked?.Invoke());
            lifetime.BracketButton(_button1, () => timescale1Clicked?.Invoke());
            lifetime.BracketButton(_button15, () => timescale15Clicked?.Invoke());
            lifetime.BracketButton(_button2, () => timescale2Clicked?.Invoke());
        }
    }
}
