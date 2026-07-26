using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CheatsPanel
{
    public class CheatsPanelView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Vector2 _openOffset;

        private Vector2 _closedPosition;
        private bool _isOpen;

        
        public void Initialize(Lifetime lifetime, Action buttonClicked)
        {
            _closedPosition = _panel.anchoredPosition;
            _isOpen = false;

            lifetime.BracketButton(_button, () => buttonClicked?.Invoke());
        }

        public void Toggle()
        {
            _isOpen = !_isOpen;
            _panel.anchoredPosition = _isOpen
                ? _closedPosition + _openOffset
                : _closedPosition;
        }
    }
}