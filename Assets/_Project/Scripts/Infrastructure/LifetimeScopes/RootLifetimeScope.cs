using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersConfig _charactersConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EventBus>(Lifetime.Singleton);
            builder.Register<PauseService>(Lifetime.Singleton);

            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_charactersConfig);
        }
    }
}