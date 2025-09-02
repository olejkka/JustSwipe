using System;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonPresenter : IStartable, IDisposable
    {
        private readonly SettingsButtonView _view;

        public bool IsPaused { get; private set; }

        public SettingsButtonPresenter(SettingsButtonView view)
        {
            _view = view;
        }

        public void Start() => _view.Clicked += OnClicked;

        public void Dispose() => _view.Clicked -= OnClicked;

        private void OnClicked()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;
        }
    }
}