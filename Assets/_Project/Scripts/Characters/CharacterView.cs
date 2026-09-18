using System;
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

        private readonly LifetimeDefinition _lifetimeDefinition = new();
        
        private Character _data;
        private Tilemap _tilemap;
        private CharacterAnimationData _animations;
        private CharacterAnimationPlayer _animationPlayer;
        private Tween _selectedJumpTween;
        private Vector3 _visualStartLocalPos;

        public CharacterAnimationType CurrentAnimationType =>
            _animationPlayer != null
                ? _animationPlayer.CurrentAnimationType
                : CharacterAnimationType.None;

        public event Action<CharacterAnimationType> OnAnimationStarted
        {
            add => AnimationPlayer.OnAnimationStarted += value;
            remove => AnimationPlayer.OnAnimationStarted -= value;
        }

        private CharacterAnimationPlayer AnimationPlayer =>
            _animationPlayer ??= new CharacterAnimationPlayer(_animator);

        
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
            AnimationPlayer.SetAnimations(animations);

            _lifetimeDefinition.Lifetime.BracketSubscription(
                () => _data.OnPositionChanged += OnMoved,
                () => _data.OnPositionChanged -= OnMoved);
            
            _lifetimeDefinition.Lifetime.BracketSubscription(
                () => _data.OnDamageTaken += OnHealthChanged,
                () => _data.OnDamageTaken -= OnHealthChanged);
            
            _lifetimeDefinition.Lifetime.BracketSubscription(
                () => _data.OnMeleeAttack += PlayMeleeAttack,
                () => _data.OnMeleeAttack -= PlayMeleeAttack);

            UpdateRotation(data.Team);
            AnimationPlayer.PlayIdle();
            UpdatePosition(_data.Position);
        }
        
        public void PlaySelected()
        {
            AnimationPlayer.TryPlayOneShot(CharacterAnimationType.Selected, _animations.Selected);
            
            _selectedJumpTween?.Kill();
            _visual.localPosition = _visualStartLocalPos;
            
            _selectedJumpTween = DOTween.Sequence()
                .Append(_visual.DOLocalMoveY(_visualStartLocalPos.y + _animations.SelectedJumpHeight, _animations.SelectedJumpDuration)
                    .SetEase(_animations.SelectedJumpEaseUp))
                .Append(_visual.DOLocalMoveY(_visualStartLocalPos.y, _animations.SelectedJumpDuration)
                    .SetEase(_animations.SelectedJumpEaseDown))
                .SetUpdate(UpdateType.Normal, isIndependentUpdate: false);
        }

        public void PlayDeath(Action onComplete)
        {
            AnimationPlayer.PlayDeath(onComplete);
        }

        private void OnMoved(Vector2Int pos)
        {
            UpdatePosition(pos);
            AnimationPlayer.TryPlayOneShot(CharacterAnimationType.Move, _animations.Move);
        }

        private void OnHealthChanged(int amount)
        {
            AnimationPlayer.TryPlayOneShot(CharacterAnimationType.TakingDamage, _animations.TakeDamage);
        }
        
        public void PlayMeleeAttack()
        {
            AnimationPlayer.TryPlayOneShot(CharacterAnimationType.MeleeAttack, _animations.MeleeAttack);
        }

        private void UpdatePosition(Vector2Int pos)
        {
            var cell = new Vector3Int(pos.x, pos.y, 0);
            transform.position = _tilemap.CellToWorld(cell);
        }
        
        private void UpdateRotation(Team team)
        {
            transform.rotation = team == Team.Player
                ? Quaternion.identity
                : Quaternion.Euler(0f, 180f, 0f);
        }

        private void OnDestroy()
        {
            _selectedJumpTween?.Kill();
            _lifetimeDefinition.Terminate();
        }
    }
}
