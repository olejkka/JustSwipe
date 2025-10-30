using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action Clicked;

        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke();
        }
    }
}