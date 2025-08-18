using System;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.InputHandlers;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Initializers
{
    public class PauseInitializer : IStartable, IDisposable
    {
        private readonly PauseKeyboardInputHandler _pauseInput;
        private readonly PauseService _pauseService;
        

        public PauseInitializer(PauseKeyboardInputHandler pauseInput, PauseService pauseService)
        {
            _pauseInput = pauseInput;
            _pauseService = pauseService;
        }

        public void Start()
        {
            _pauseInput.OnPauseButtonPressed += OnToggle;
        }

        public void Dispose()
        {
            _pauseInput.OnPauseButtonPressed -= OnToggle;
        }

        private void OnToggle()
        {
            if (_pauseService.IsPaused)
                _pauseService.RequestResume();
            else 
                _pauseService.RequestPause();
        }
    }
}