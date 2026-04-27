using System;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class PauseService
    {
        private bool _isPaused;
        public bool IsPaused => _isPaused;
        
        
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
        }
    }
}