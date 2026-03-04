using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Audio;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsPopup
{
    public class SettingsPopupPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly SettingsPopupView _view;
        private readonly AudioService _audioService;

        private bool _isPopupOpen;

        
        public SettingsPopupPresenter(
            EventBus eventBus,
            SettingsPopupView view,
            AudioService audioService
            )
        {
            _eventBus = eventBus;
            _view = view;
            _audioService = audioService;
        }

        public void Start()
        {
            _eventBus.Subscribe<SettingsButtonToggleEvent>(OnSettingsButtonToggle);
            
            _view.ClosureAreaClicked += OnClosureAreaClicked;
            _view.ReturnToMenuClicked += OnReturnToMenuClicked;
            _view.ToggleSoundClicked += OnToggleSoundClicked;
            
            _view.ToggleSoundIcons(_audioService.Muted);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SettingsButtonToggleEvent>(OnSettingsButtonToggle);
            
            _view.ClosureAreaClicked -= OnClosureAreaClicked;
            _view.ReturnToMenuClicked -= OnReturnToMenuClicked;
            _view.ToggleSoundClicked -= OnToggleSoundClicked;

        }

        private void OnSettingsButtonToggle(SettingsButtonToggleEvent _)
        {
            _isPopupOpen = !_isPopupOpen;
            _view.ToggleActive(_isPopupOpen);
        }

        private void OnClosureAreaClicked()
        {
            _isPopupOpen = false;
            _view.ToggleActive(false);
        }

        private void OnReturnToMenuClicked()
        {
            _isPopupOpen = false;
            _view.ToggleActive(false);
            _eventBus.Publish(new ReturnToMenuEvent());
        }

        private void OnToggleSoundClicked()
        {
            _eventBus.Publish(new ToggleMuteEvent());
            _view.ToggleSoundIcons(_audioService.Muted);
        }
    }
}