using System;
using _Project.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters.Projectiles
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private SpriteAnimator _animator;
        [SerializeField] private Transform _visual;

        private Tween _moveTween;


        public void Play(
            Projectile projectile,
            Tilemap tilemap,
            ProjectileAnimationData animations,
            Action onComplete)
        {
            Stop();

            transform.position = CellToWorld(tilemap, projectile.StartPosition);
            _visual.rotation = ShouldFlip(projectile.Direction)
                ? Quaternion.Euler(0f, 180f, 0f)
                : Quaternion.identity;

            _animator.Play(animations.Projectile, animations.FrameRate, loop: true);

            var end = CellToWorld(tilemap, projectile.EndPosition);
            var distance = Vector3.Distance(transform.position, end);
            var duration = animations.Speed <= 0f ? 0f : distance / animations.Speed;

            _moveTween = transform
                .DOMove(end, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _animator.Stop();
                    onComplete?.Invoke();
                });
        }

        public void Stop()
        {
            _moveTween?.Kill();
            _moveTween = null;
            _animator.Stop();
        }

        private void OnDestroy() => Stop();

        private static Vector3 CellToWorld(Tilemap tilemap, Vector2Int pos) =>
            tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, 0));

        private static bool ShouldFlip(Vector2Int direction) =>
            direction.x < 0 || direction.y > 0;
    }
}
