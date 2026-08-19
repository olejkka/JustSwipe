using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Projectiles;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace _Project.Scripts.Instantiators
{
    public class ProjectileViewInstantiator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private ProjectileView _projectileViewPrefab;

        [Inject] private ProjectilesConfig _projectilesConfig;
        [Inject] private EventBus _eventBus;

        private readonly LifetimeDefinition _lifetimeDefinition = new();
        private readonly Stack<ProjectileView> _pool = new();
        private readonly HashSet<ProjectileView> _active = new();


        [Inject]
        public void Initialize()
        {
            _eventBus.SubscribeWithLifetime<ProjectileCreatedEvent>(
                _lifetimeDefinition.Lifetime,
                OnProjectileCreated);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();

            foreach (var view in _active)
            {
                if (view != null)
                    Destroy(view.gameObject);
            }

            while (_pool.Count > 0)
            {
                var view = _pool.Pop();
                if (view != null)
                    Destroy(view.gameObject);
            }

            _active.Clear();
        }

        private void OnProjectileCreated(ProjectileCreatedEvent e)
        {
            var definition = _projectilesConfig.GetEntryByDefinitionId(e.Projectile.DefinitionId);

            if (definition?.Animations == null ||
                definition.Animations.Projectile == null ||
                definition.Animations.Projectile.Length == 0)
            {
                Debug.LogError($"No projectile animations found for {e.Projectile.DefinitionId}");
                return;
            }

            var view = Get();
            _active.Add(view);
            view.Play(e.Projectile, _tilemap, definition.Animations, () => Release(view));
        }

        private ProjectileView Get()
        {
            while (_pool.Count > 0)
            {
                var pooled = _pool.Pop();
                if (pooled == null)
                    continue;

                pooled.gameObject.SetActive(true);
                return pooled;
            }

            return Instantiate(_projectileViewPrefab, transform);
        }

        private void Release(ProjectileView view)
        {
            if (view == null)
                return;

            _active.Remove(view);
            view.Stop();
            view.gameObject.SetActive(false);
            view.transform.SetParent(transform, false);
            _pool.Push(view);
        }
    }
}
