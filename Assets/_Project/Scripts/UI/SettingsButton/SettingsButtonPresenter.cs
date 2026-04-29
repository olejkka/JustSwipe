using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly SettingsButtonView _view;

        public SettingsButtonPresenter(
            EventBus eventBus,
            SettingsButtonView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start() => _view.SettingsClicked += OnSettingsClicked;
        public void Dispose() => _view.SettingsClicked -= OnSettingsClicked;
        private void OnSettingsClicked() => _eventBus.Publish(new SettingsButtonToggleEvent());
    }
}