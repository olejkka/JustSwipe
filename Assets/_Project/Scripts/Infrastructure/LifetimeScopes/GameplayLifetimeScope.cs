using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using _Project.Scripts.UI;
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
            builder.RegisterEntryPoint<GameplayEntryPoint>();
            
            builder.Register<CharacterSpawnController>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);
            builder.Register<CharacterDeathHandler>(Lifetime.Singleton);
            
            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            builder.Register<IGameplayStatesProvider, GameplayStatesProvider>(Lifetime.Singleton);
            builder.Register<TurnService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<PauseButtonPresenter>();

            builder.RegisterComponentInHierarchy<PauseButtonView>();
            
            RegisterInputHandlers(builder);
            RegisterConfigs(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
        }
        
        private void RegisterInputHandlers(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<KeyboardInputHandler>();
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
            builder.RegisterComponentInHierarchy<BotInputHandler>();
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
            builder.Register<CharactersViewsStorage>(Lifetime.Singleton);
        }
    }
}