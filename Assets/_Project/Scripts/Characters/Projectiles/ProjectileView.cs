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

        private Tween _moveTween;


        public void Play(
            Projectile projectile,
            Tilemap tilemap,
            ProjectileAnimationData animations,
            Action onComplete)
        {
            Stop();

            transform.position = CellToWorld(tilemap, projectile.StartPosition);
            transform.rotation = DirectionToRotation(projectile.Direction);

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

        private static Quaternion DirectionToRotation(Vector2Int direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, angle);
        }
    }
}
