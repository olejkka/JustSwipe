using System;
using UnityEngine;
using _Project.Scripts.InputHandlers;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class PauseButtonPresenter : IStartable, IDisposable
    {
        private readonly PauseButtonView _view;
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotInputHandler _botInputHandler;
        
        private bool _isPaused;

        public PauseButtonPresenter(
            PauseButtonView view,
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler)
        {
            _view = view;
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
        }

        public void Start()
        {
            if (_view != null)
                _view.Clicked += OnClicked;
        }

        public void Dispose()
        {
            if (_view != null)
                _view.Clicked -= OnClicked;
        }

        private void OnClicked()
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;
            
            _keyboardInputHandler.enabled = false;
            _swipeInputHandler.enabled = false;
            _botInputHandler.enabled = false;
        }

        private void Resume()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            
            _keyboardInputHandler.enabled = true;
            _swipeInputHandler.enabled = true;
            _botInputHandler.enabled = true;
        }
    }
}