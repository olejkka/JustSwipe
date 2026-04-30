using System;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.SettingsButton
{
    public class SettingsButtonPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly SettingsButtonView _view;
        
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public SettingsButtonPresenter(
            EventBus eventBus,
            SettingsButtonView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start()
        {
            _view.Initialize(_lifetimeDefinition.Lifetime, OnSettingsClicked);
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnSettingsClicked()
        {
            _eventBus.Publish(new SettingsButtonToggleEvent());
        }
    }
}