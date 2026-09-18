using System;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharacterAnimationPlayer
    {
        private readonly SpriteAnimator _animator;

        private CharacterAnimationData _animations;
        private CharacterAnimationType _currentAnimationType = CharacterAnimationType.None;
        private bool _isOneShotPlaying;
        private int _currentPriority;

        public CharacterAnimationType CurrentAnimationType => _currentAnimationType;

        public event Action<CharacterAnimationType> OnAnimationStarted;


        public CharacterAnimationPlayer(SpriteAnimator animator)
        {
            _animator = animator;
        }

        public void SetAnimations(CharacterAnimationData animations)
        {
            _animations = animations;
        }

        public void PlayIdle()
        {
            _isOneShotPlaying = false;
            _currentAnimationType = CharacterAnimationType.Idle;
            _currentPriority = (int)CharacterAnimationType.Idle;

            OnAnimationStarted?.Invoke(CharacterAnimationType.Idle);

            _animator.Play(_animations.Idle, _animations.FrameRate, loop: true);
        }

        public void TryPlayOneShot(
            CharacterAnimationType animationType,
            Sprite[] frames,
            Action onComplete = null,
            bool returnToIdleOnFinish = true)
        {
            var newPriority = (int)animationType;

            if (!_isOneShotPlaying || newPriority > _currentPriority)
                PlayOneShotInternal(animationType, frames, onComplete, returnToIdleOnFinish);
        }

        public void PlayDeath(Action onComplete)
        {
            var frames = _animations != null ? _animations.Death : null;

            if (frames.Length == 0)
            {
                onComplete?.Invoke();
                return;
            }

            TryPlayOneShot(
                CharacterAnimationType.Death,
                frames,
                onComplete,
                returnToIdleOnFinish: false);
        }

        public void Stop()
        {
            _isOneShotPlaying = false;
            _currentAnimationType = CharacterAnimationType.None;
            _currentPriority = (int)CharacterAnimationType.None;
            _animator.Stop();
        }

        private void PlayOneShotInternal(
            CharacterAnimationType animationType,
            Sprite[] frames,
            Action onComplete,
            bool returnToIdleOnFinish)
        {
            if (frames.Length == 0)
            {
                onComplete?.Invoke();

                if (returnToIdleOnFinish)
                    PlayIdle();

                return;
            }

            _isOneShotPlaying = true;
            _currentAnimationType = animationType;
            _currentPriority = (int)animationType;

            OnAnimationStarted?.Invoke(animationType);

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
    }
}
