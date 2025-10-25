using System;
using UnityEngine;
using _Project.Scripts.InputHandlers;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class PauseButtonPresenter : IStartable, IDisposable
    {
        private readonly PauseButtonView _view;
        private readonly SwipeInputHandler _swipeInputHandler;
        
        private bool _isPaused;

        public PauseButtonPresenter(
            PauseButtonView view,
            SwipeInputHandler swipeInputHandler
            )
        {
            _view = view;
            _swipeInputHandler = swipeInputHandler;
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
            
            _swipeInputHandler.enabled = false;
        }

        private void Resume()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            
            _swipeInputHandler.enabled = true;
        }
    }
}