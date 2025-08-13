using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.Initializers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        [SerializeField] private TileInstantiator _tileInstantiator;

        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputInitializer>();
            builder.RegisterEntryPoint<GameplayInitializer>();
            
            builder.Register<CharacterSpawnController>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);
            
            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            builder.Register<IGameplayStatesProvider, GameplayStatesProvider>(Lifetime.Singleton);

            builder.Register<PhaseHandler>(Lifetime.Singleton).As<IStartable>().AsSelf();
            builder.Register<InputStorage>(Lifetime.Singleton);
            
            builder.Register<InputReadingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            builder.Register<CharactersMovingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            
            RegisterConfigs(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
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