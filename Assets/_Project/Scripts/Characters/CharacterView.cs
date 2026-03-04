using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteAnimator _animator;

        private EventBus _eventBus;
        private Character _data;
        private Tilemap _tilemap;
        private CharacterAnimationData _animations;

        private bool _isOneShotPlaying;
        private CharacterAnimationType _currentAnimationType = CharacterAnimationType.None;
        private int _currentPriority;
        

        public void Init(
            Character data, 
            Tilemap tilemap, 
            CharacterAnimationData animations, 
            EventBus eventBus
            )
        {
            _data = data;
            _tilemap = tilemap;
            _animations = animations;
            _eventBus = eventBus;

            _data.OnPositionChanged += OnMoved;
            _data.OnHealthChanged += OnHealthChanged;
            _eventBus.Subscribe<SwipeEvent>(OnSwipe);

            PlayIdle();
            UpdatePosition(_data.Position);
        }
        
        private void OnSwipe(SwipeEvent e)
        {
            if (e.Direction.x > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (e.Direction.x < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        private void PlayIdle()
        {
            _isOneShotPlaying = false;
            _currentAnimationType = CharacterAnimationType.Idle;
            _currentPriority = (int)CharacterAnimationType.Idle;

            _animator.Play(_animations.Idle, _animations.FrameRate, loop: true);
        }

        private void TryPlayOneShot(
            CharacterAnimationType animationType,
            Sprite[] frames,
            Action onComplete = null,
            bool returnToIdleOnFinish = true)
        {
            var newPriority = (int)animationType;
            
            if (!_isOneShotPlaying)
            {
                PlayOneShotInternal(animationType, frames, onComplete, returnToIdleOnFinish);
                return;
            }
            
            if (newPriority > _currentPriority)
            {
                PlayOneShotInternal(animationType, frames, onComplete, returnToIdleOnFinish);
            }
        }

        private void PlayOneShotInternal(
            CharacterAnimationType animationType,
            Sprite[] frames,
            Action onComplete,
            bool returnToIdleOnFinish)
        {
            _isOneShotPlaying = true;
            _currentAnimationType = animationType;
            _currentPriority = (int)animationType;

            _animator.Play(frames, _animations.FrameRate, loop: false);
            
            _animator.OnAnimationFinished += () =>
            {
                _isOneShotPlaying = false;
                _currentAnimationType = CharacterAnimationType.None;
                _currentPriority = (int)CharacterAnimationType.None;

                onComplete?.Invoke();

                if (returnToIdleOnFinish)
                    PlayIdle();
            };
        }

        public void PlayDeath(Action onComplete)
        {
            TryPlayOneShot(
                CharacterAnimationType.Death,
                _animations.Death,
                onComplete,
                returnToIdleOnFinish: false);
        }

        private void OnMoved(Vector2Int pos)
        {
            UpdatePosition(pos);
            TryPlayOneShot(CharacterAnimationType.Move, _animations.Move);
        }

        private void OnHealthChanged(int health)
        {
            TryPlayOneShot(CharacterAnimationType.TakingDamage, _animations.TakeDamage);
        }

        private void UpdatePosition(Vector2Int pos)
        {
            var cell = new Vector3Int(pos.x, pos.y, 0);
            transform.position = _tilemap.CellToWorld(cell);
        }

        private void OnDestroy()
        {
            _data.OnPositionChanged -= OnMoved;
            _data.OnHealthChanged -= OnHealthChanged;
            _eventBus?.Unsubscribe<SwipeEvent>(OnSwipe);
        }
    }
}