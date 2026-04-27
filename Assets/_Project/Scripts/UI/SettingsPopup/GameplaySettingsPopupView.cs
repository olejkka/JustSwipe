using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SettingsPopup
{
    public class GameplaySettingsPopupView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Button _closureArea;
        [SerializeField] private Button _returnToMenuButton;
        [SerializeField] private Button _toggleSoundOffButton;
        [SerializeField] private Button _toggleSoundOnButton;

        public event Action ClosureAreaClicked;
        public event Action ReturnToMenuClicked;
        public event Action ToggleSoundClicked;

        
        private void Awake()
        {
            ToggleActive(false);
        }

        private void OnEnable()
        {
            _closureArea.onClick.AddListener(OnClosureAreaClicked);
            _returnToMenuButton?.onClick.AddListener(OnReturnToMenuClicked);
            _toggleSoundOffButton.onClick.AddListener(OnToggleSoundClicked);
            _toggleSoundOnButton.onClick.AddListener(OnToggleSoundClicked);
        }

        private void OnDisable()
        {
            _closureArea.onClick.RemoveListener(OnClosureAreaClicked);
            _returnToMenuButton?.onClick.RemoveListener(OnReturnToMenuClicked);
            _toggleSoundOffButton.onClick.RemoveListener(OnToggleSoundClicked);
            _toggleSoundOnButton.onClick.RemoveListener(OnToggleSoundClicked);
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

        private void OnClosureAreaClicked() => ClosureAreaClicked?.Invoke();
        private void OnReturnToMenuClicked() => ReturnToMenuClicked?.Invoke();
        private void OnToggleSoundClicked() => ToggleSoundClicked?.Invoke();
    }
}