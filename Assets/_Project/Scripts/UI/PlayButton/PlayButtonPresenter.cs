using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.PlayButton
{
    public class PlayButtonPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly PlayButtonView _view;

        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        
        public PlayButtonPresenter(EventBus eventBus, PlayButtonView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start()
        {
            _view.Initialize(_lifetimeDefinition.Lifetime, OnPlayClicked);
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnPlayClicked()
        {
            _eventBus.Publish(new PlayClickedEvent());
        }
    }
}