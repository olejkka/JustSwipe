using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class ApplicationQuitHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;

        public ApplicationQuitHandler(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Start()
        {
            _eventBus.Subscribe<ApplicationQuitRequestedEvent>(OnApplicationQuitRequested);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<ApplicationQuitRequestedEvent>(OnApplicationQuitRequested);
        }

        private void OnApplicationQuitRequested(ApplicationQuitRequestedEvent e)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}