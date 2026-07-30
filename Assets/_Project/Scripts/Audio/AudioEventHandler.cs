using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Audio
{
    public sealed class AudioEventHandler : IStartable, IDisposable
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly AudioService _audioService;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public AudioEventHandler(EventBus.EventBus eventBus, AudioService audioService)
        {
            _eventBus = eventBus;
            _audioService = audioService;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<MenuEnteredEvent>(_lifetimeDefinition.Lifetime, OnMenuEnteredEvent);
            _eventBus.SubscribeWithLifetime<StartGameplayEvent>(_lifetimeDefinition.Lifetime, OnStartGameplayEvent);
            _eventBus.SubscribeWithLifetime<SwipeEvent>(_lifetimeDefinition.Lifetime, OnSwipe);
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(_lifetimeDefinition.Lifetime, OnCharacterDied);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        private void OnMenuEnteredEvent(MenuEnteredEvent e)
        {
            _audioService.PlayMusic(SoundId.MenuMusic);
        }
        
        private void OnStartGameplayEvent(StartGameplayEvent e)
        {
            _audioService.PlayMusic(SoundId.GameplayMusic);
        }
        
        private void OnSwipe(SwipeEvent e)
        {
            _audioService.PlaySfx(SoundId.Swipe);
        }

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            var p = e.Character.Position;
            var worldPos = new Vector3(p.x, p.y, 0f);
            _audioService.PlaySfxAt(SoundId.CharacterDeath, worldPos);
        }
    }
}