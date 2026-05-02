using System;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class ApplicationQuitHandler : IStartable, IDisposable
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        

        public ApplicationQuitHandler(EventBus.EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<ApplicationQuitRequestedEvent>(_lifetimeDefinition.Lifetime, OnApplicationQuitRequested);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
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