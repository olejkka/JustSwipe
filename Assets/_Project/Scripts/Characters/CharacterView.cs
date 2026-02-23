using System;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteAnimator _animator;

        private Character _data;
        private Tilemap _tilemap;
        private CharacterAnimationData _animations;
        private bool _isDead;

        public void Init(Character data, Tilemap tilemap, CharacterAnimationData animations)
        {
            _data = data;
            _tilemap = tilemap;
            _animations = animations;

            _data.OnPositionChanged += OnMoved;
            _data.OnHealthChanged += OnHealthChanged;

            PlayIdle();
            UpdatePosition(_data.Position);
        }

        private void PlayIdle()
        {
            if (_isDead) 
                return;
            
            _animator.Play(_animations.Idle, _animations.FrameRate, loop: true);
        }

        private void PlayOneShot(Sprite[] frames)
        {
            if (_isDead || frames == null || frames.Length == 0) return;
            _animator.Play(frames, _animations.FrameRate, loop: false);
            _animator.OnAnimationFinished += PlayIdle;
        }

        public void PlayDeath(Action onComplete)
        {
            _isDead = true;

            if (_animations.Death is { Length: > 0 })
            {
                _animator.Play(_animations.Death, _animations.FrameRate, loop: false);
                _animator.OnAnimationFinished += () => onComplete?.Invoke();
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        private void OnMoved(Vector2Int pos)
        {
            UpdatePosition(pos);
            PlayOneShot(_animations.Move);
        }

        private void OnHealthChanged(int health)
        {
        }

        private void UpdatePosition(Vector2Int pos)
        {
            var cell = new Vector3Int(pos.x, pos.y, 0);
            transform.position = _tilemap.CellToWorld(cell);
        }

        private void OnDestroy()
        {
            if (_data == null) return;
            _data.OnPositionChanged -= OnMoved;
            _data.OnHealthChanged -= OnHealthChanged;
        }
    }
}