using System;
using _Project.Scripts.Infrastructure.FSM;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class PauseButtonPresenter : IStartable, IDisposable
    {
        private readonly PauseButtonView _view;
        private readonly PauseService _pauseService;

        public PauseButtonPresenter(PauseButtonView view, PauseService pauseService)
        {
            _view = view;
            _pauseService = pauseService;
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
            if (_pauseService.IsPaused)
                _pauseService.RequestResume();
            else
                _pauseService.RequestPause();
        }
    }
}