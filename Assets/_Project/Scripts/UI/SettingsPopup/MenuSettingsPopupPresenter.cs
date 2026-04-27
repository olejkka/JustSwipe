using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Audio;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsPopup
{
    public class MenuSettingsPopupPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly MenuSettingsPopupView _view;
        private readonly AudioService _audioService;
        private readonly PauseService _pauseService;

        private bool _isPopupOpen;

        
        public MenuSettingsPopupPresenter(
            EventBus eventBus,
            MenuSettingsPopupView view,
            AudioService audioService,
            PauseService pauseService
            )
        {
            _eventBus = eventBus;
            _view = view;
            _audioService = audioService;
            _pauseService = pauseService;
        }

        public void Start()
        {
            _eventBus.Subscribe<SettingsButtonToggleEvent>(OnSettingsButtonToggle);
            
            _view.ClosureAreaClicked += OnClosureAreaClicked;
            _view.ApplicationQuitClicked += OnApplicationQuitClicked;
            _view.ToggleSoundClicked += OnToggleSoundClicked;
            
            _view.ToggleSoundIcons(_audioService.Muted);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SettingsButtonToggleEvent>(OnSettingsButtonToggle);
            
            _view.ClosureAreaClicked -= OnClosureAreaClicked;
            _view.ApplicationQuitClicked -= OnApplicationQuitClicked;
            _view.ToggleSoundClicked -= OnToggleSoundClicked;
        }

        private void OnSettingsButtonToggle(SettingsButtonToggleEvent _)
        {
            if (_isPopupOpen)
                Close();
            else
                Open();
        }

        private void Open()
        {
            _isPopupOpen = true;
            _view.ToggleActive(true);
            _pauseService.Pause();
        }
        
        private void Close()
        {
            _isPopupOpen = false;
            _view.ToggleActive(false);
            _pauseService.Resume();
        }
        
        private void OnClosureAreaClicked()
        {
            Close();
        }
        
        private void OnApplicationQuitClicked()
        {
            _eventBus.Publish(new ApplicationQuitRequestedEvent());
        }

        private void OnToggleSoundClicked()
        {
            _audioService.SetMuted(!_audioService.Muted);
            _view.ToggleSoundIcons(_audioService.Muted);
        }
    }
}