using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action SettingsClicked;

        
        private void OnEnable() => _button.onClick.AddListener(OnSettingsClicked);
        private void OnDisable() => _button.onClick.RemoveListener(OnSettingsClicked);
        private void OnSettingsClicked() => SettingsClicked?.Invoke();
    }
}