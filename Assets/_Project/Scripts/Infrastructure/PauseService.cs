using System;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class PauseService
    {
        private const float InputBlockAfterResumeSeconds = 0.1f;
        
        private bool _isPaused;
        private float _inputBlockedUntil;
        
        public bool IsPaused => _isPaused;
        public bool IsGameplayInputBlocked => _isPaused || Time.unscaledTime < _inputBlockedUntil;
        
        
        public void Pause()
        {
            if (_isPaused)
                return;
            
            _isPaused = true;
            Time.timeScale = 0f;
        }
        
        public void Resume()
        {
            if (!_isPaused)
                return;
            
            _isPaused = false;
            Time.timeScale = 1f;
            _inputBlockedUntil = Time.unscaledTime + InputBlockAfterResumeSeconds;
        }
    }
}