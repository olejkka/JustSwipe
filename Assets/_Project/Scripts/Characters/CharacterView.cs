using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.Utilities;
using DG.Tweening;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteAnimator _animator;
        [SerializeField] private Transform _visual;
        
        [Header("Jump Animation")]
        [SerializeField] private float _selectedJumpHeight;
        [SerializeField] private float _selectedJumpDuration;
        [SerializeField] private Ease _selectedJumpEaseUp = Ease.OutQuad;
        [SerializeField] private Ease _selectedJumpEaseDown = Ease.InQuad;

        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        private Character _data;
        private Tilemap _tilemap;
        private CharacterAnimationData _animations;
        private CharacterAnimationType _currentAnimationType = CharacterAnimationType.None;
        private bool _isOneShotPlaying;
        private int _currentPriority;
        private Tween _selectedJumpTween;
        private Vector3 _visualStartLocalPos;
        

        private void Awake()
        {
            _visualStartLocalPos = _visual.localPosition;
        }
        
        public void Init(
            Character data, 
            Tilemap tilemap, 
            CharacterAnimationData animations)
        {
            _data = data;
            _tilemap = tilemap;
            _animations = animations;

            _lifetimeDefinition.Lifetime.BracketSubscription(
                () => _data.OnPositionChanged += OnMoved,
                () => _data.OnPositionChanged -= OnMoved);
            
            _lifetimeDefinition.Lifetime.BracketSubscription(
                () => _data.OnHealthChanged += OnHealthChanged,
                () => _data.OnHealthChanged -= OnHealthChanged);

            PlayIdle();
            UpdatePosition(_data.Position);
        }

        private void PlayIdle()
        {
            _isOneShotPlaying = false;
            _currentAnimationType = CharacterAnimationType.Idle;
            _currentPriority = (int)CharacterAnimationType.Idle;

            _animator.Play(_animations.Idle, _animations.FrameRate, loop: true);
        }
        
        public void PlaySelected()
        {
            TryPlayOneShot(CharacterAnimationType.Selected, _animations.Selected);
            
            _selectedJumpTween?.Kill();
            _visual.localPosition = _visualStartLocalPos;
            
            _selectedJumpTween = DOTween.Sequence()
                .Append(_visual.DOLocalMoveY(_visualStartLocalPos.y + _selectedJumpHeight, _selectedJumpDuration)
                    .SetEase(_selectedJumpEaseUp))
                .Append(_visual.DOLocalMoveY(_visualStartLocalPos.y, _selectedJumpDuration)
                    .SetEase(_selectedJumpEaseDown))
                .SetUpdate(UpdateType.Normal, isIndependentUpdate: false);
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

        private void OnMoved(Vector2Int pos, Vector2Int direction)
        {
            UpdateRotation(direction);
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
        
        private void UpdateRotation(Vector2Int direction)
        {
            if (direction.x > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (direction.x < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        private void OnDestroy()
        {
            _selectedJumpTween?.Kill();
            _lifetimeDefinition.Terminate();
        }
    }
}