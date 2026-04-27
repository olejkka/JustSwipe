using System;
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

        public event Action ClosureAreaClicked;
        public event Action ApplicationQuitClicked;
        public event Action ToggleSoundClicked;


        private void Awake()
        {
            ToggleActive(false);
        }

        private void OnEnable()
        {
            _closureArea.onClick.AddListener(OnClosureAreaClicked);
            _applicationQuitButton?.onClick.AddListener(OnApplicationQuitClicked);
            _toggleSoundOffButton.onClick.AddListener(OnToggleSoundClicked);
            _toggleSoundOnButton.onClick.AddListener(OnToggleSoundClicked);
        }

        private void OnDisable()
        {
            _closureArea.onClick.RemoveListener(OnClosureAreaClicked);
            _applicationQuitButton?.onClick.RemoveListener(OnApplicationQuitClicked);
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
        private void OnApplicationQuitClicked() => ApplicationQuitClicked?.Invoke();
        private void OnToggleSoundClicked() => ToggleSoundClicked?.Invoke();
    }
}