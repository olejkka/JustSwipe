using System;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        private Sprite[] _frames;
        private float _frameRate;
        private float _timer;
        private int _currentFrame;
        private bool _loop;
        private bool _playing;

        public event Action OnAnimationFinished;

        public void Play(Sprite[] frames, float frameRate, bool loop = true)
        {
            if (frames == null || frames.Length == 0)
                return;

            OnAnimationFinished = null;

            _frames = frames;
            _frameRate = frameRate;
            _loop = loop;
            _currentFrame = 0;
            _timer = 0f;
            _playing = true;
            _renderer.sprite = _frames[0];
        }

        public void Stop()
        {
            _playing = false;
        }

        private void Update()
        {
            if (!_playing || _frames == null || (_loop && _frames.Length <= 1))
                return;

            _timer += Time.deltaTime;

            if (_timer < 1f / _frameRate)
                return;

            _timer = 0f;
            _currentFrame++;

            if (_currentFrame >= _frames.Length)
            {
                if (_loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _playing = false;
                    OnAnimationFinished?.Invoke();
                    return;
                }
            }

            _renderer.sprite = _frames[_currentFrame];
        }
    }
}