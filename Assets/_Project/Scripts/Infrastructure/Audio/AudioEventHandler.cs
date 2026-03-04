using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Audio
{
    public sealed class AudioEventHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly AudioService _audioService;

        
        public AudioEventHandler(EventBus eventBus, AudioService audioService)
        {
            _eventBus = eventBus;
            _audioService = audioService;
        }

        public void Start()
        {
            _eventBus.Subscribe<MenuEnteredEvent>(OnMenuEnteredEvent);
            _eventBus.Subscribe<StartGameplayEvent>(OnStartGameplayEvent);
            _eventBus.Subscribe<ToggleMuteEvent>(OnToggleSoundEvent);
            _eventBus.Subscribe<SwipeEvent>(OnSwipe);
            _eventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<MenuEnteredEvent>(OnMenuEnteredEvent);
            _eventBus.Unsubscribe<StartGameplayEvent>(OnStartGameplayEvent);
            _eventBus.Unsubscribe<ToggleMuteEvent>(OnToggleSoundEvent);
            _eventBus.Unsubscribe<SwipeEvent>(OnSwipe);
            _eventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);
        }

        private void OnMenuEnteredEvent(MenuEnteredEvent e)
        {
            _audioService.PlayMusic(SoundId.MenuMusic);
        }
        
        private void OnStartGameplayEvent(StartGameplayEvent e)
        {
            _audioService.PlayMusic(SoundId.GameplayMusic);
        }

        private void OnToggleSoundEvent(ToggleMuteEvent e)
        {
            _audioService.SetMuted(!_audioService.Muted);
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