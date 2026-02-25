using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsPopup
{
    public class SettingsPopupPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly SettingsPopupView _view;

        private bool _isPopupOpen;

        public SettingsPopupPresenter(
            EventBus eventBus,
            SettingsPopupView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start()
        {
            _eventBus.Subscribe<SettingsButtonToggleEvent>(OnSettingsButtonToggle);
            
            _view.ClosureAreaClicked += OnClosureAreaClicked;
            _view.ReturnToMenuClicked += OnReturnToMenuClicked;
            _view.ToggleSoundClicked += OnToggleSoundClicked;
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
            _eventBus.Publish(new ToggleSoundEvent());
        }
    }
}