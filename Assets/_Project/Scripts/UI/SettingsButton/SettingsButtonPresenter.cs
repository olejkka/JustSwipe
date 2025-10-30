using System;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonPresenter : IStartable, IDisposable
    {
        private readonly SettingsButtonView _view;
        private readonly PauseService _pauseService;

        
        public SettingsButtonPresenter(
            SettingsButtonView view,
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