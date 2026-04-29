using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.PlayButton
{
    public class PlayButtonPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly PlayButtonView _view;

        
        public PlayButtonPresenter(EventBus eventBus, PlayButtonView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start()
        {
            _view.Clicked += OnPlayClicked;
        }

        public void Dispose()
        {
            _view.Clicked -= OnPlayClicked;
        }

        private void OnPlayClicked()
        {
            _eventBus.Publish(new PlayClickedEvent());
        }
    }
}