using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SettingsPopup
{
    public class MenuSettingsPopupView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Button _closureArea;
        [SerializeField] private Button _applicationQuitButton;
        [SerializeField] private Button _toggleSoundOffButton;
        [SerializeField] private Button _toggleSoundOnButton;

        
        public void Initialize(
            Lifetime lifetime,
            Action closureAreaClicked,
            Action applicationQuitClicked,
            Action toggleSoundClicked)
        {
            lifetime.BracketButton(_closureArea, () => closureAreaClicked?.Invoke());
            lifetime.BracketButton(_applicationQuitButton, () => applicationQuitClicked?.Invoke());
            lifetime.BracketButton(_toggleSoundOffButton, () => toggleSoundClicked?.Invoke());
            lifetime.BracketButton(_toggleSoundOnButton, () => toggleSoundClicked?.Invoke());
        }

        private void Awake()
        {
            ToggleActive(false);
        }

        public void ToggleSoundIcons(bool muted)
        {
            _toggleSoundOffButton.gameObject.SetActive(!muted);
            _toggleSoundOnButton.gameObject.SetActive(muted);
        }

        public void ToggleActive(bool flag)
        {
            _container.SetActive(flag);
            _closureArea.gameObject.SetActive(flag);
        }
    }
}