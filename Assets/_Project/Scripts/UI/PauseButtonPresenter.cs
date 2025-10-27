using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.UI;
using VContainer.Unity;

namespace _Project.Scripts.UI
{
    public class PauseButtonPresenter : IStartable, IDisposable
    {
        private readonly PauseButtonView _view;
        private readonly PauseService _pauseService;

        
        public PauseButtonPresenter(
            PauseButtonView view,
            PauseService pauseService)
        {
            _view = view;
            _pauseService = pauseService;
        }

        public void Start()
        {
            _view.Clicked += _pauseService.TogglePause;
        }

        public void Dispose()
        {
            _view.Clicked -= _pauseService.TogglePause;
        }
    }
}