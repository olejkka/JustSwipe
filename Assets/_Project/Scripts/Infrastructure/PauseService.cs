using System;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class PauseService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private bool _isPaused;

        public bool IsPaused => _isPaused;

        public event Action OnPaused;
        public event Action OnResumed;


        public PauseService(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Start()
        {
            _eventBus.Subscribe<SettingsButtonToggleEvent>(TogglePause);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SettingsButtonToggleEvent>(TogglePause);
        }

        private void TogglePause(SettingsButtonToggleEvent e)
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }

        private void Pause()
        {
            if (_isPaused)
                return;

            _isPaused = true;
            Time.timeScale = 0f;
            OnPaused?.Invoke();
        }

        private void Resume()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            Time.timeScale = 1f;
            OnResumed?.Invoke();
        }
    }
}