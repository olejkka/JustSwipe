using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayConfigs _gameplayConfigs;

        protected override void Configure(IContainerBuilder builder)
        {
            _gameplayConfigs.RegisterAll(builder);
            
            builder.Register<EventBus>(Lifetime.Singleton);
            builder.Register<PauseService>(Lifetime.Singleton);
        }
    }
}