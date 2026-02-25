using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PlayButton
{
    public class PlayButtonView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        public event Action Clicked;

        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked() => Clicked?.Invoke();
    }
}