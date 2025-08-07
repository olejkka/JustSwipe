using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        [SerializeField] private TileInstantiator _tileInstantiator;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameInitializer>();
            
            builder.Register<CharacterSpawnController>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);

            RegisterInput(builder);
            RegisterConfigs(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
        }

        private void RegisterInput(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<KeyboardInputHandler>();
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();

            builder.Register<IInputHandler>(
                resolver => Application.isMobilePlatform
                    ? resolver.Resolve<SwipeInputHandler>()
                    : resolver.Resolve<KeyboardInputHandler>(),
                Lifetime.Singleton
            );
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);
        }

        private void RegisterCreators(IContainerBuilder builder)
        {
            builder.Register<PositionsCreator>(Lifetime.Singleton);
            builder.Register<CharacterCreator>(Lifetime.Singleton);
        }

        private void RegisterInstantiators(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tileInstantiator);
            builder.RegisterComponentInHierarchy<CharacterViewInstantiator>();
        }

        private void RegisterStorages(IContainerBuilder builder)
        {
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
        }
    }
}