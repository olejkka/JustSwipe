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
using _Project.Scripts.Wallet;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersConfig _charactersConfig;
        
        [SerializeField] private TileInstantiator _tileInstantiator;

        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
            
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
            
            builder.Register<Money>(Lifetime.Singleton);
            
            builder.Register<CharacterSpawnController>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);
            builder.Register<CharacterDeathHandler>(Lifetime.Singleton);
            
            builder.Register<IGameplayStatesProvider, GameplayStatesProvider>(Lifetime.Singleton);
            builder.Register<TurnService>(Lifetime.Singleton);
            
            builder.Register<PauseService>(Lifetime.Singleton);
            
            RegisterConfigs(builder);
            RegisterPresenters(builder);
            RegisterViews(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PauseButtonView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseView>();
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PauseButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
            builder.RegisterEntryPoint<CharacterPurchaseCasePresenter>();
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_charactersConfig);
        }

        private void RegisterCreators(IContainerBuilder builder)
        {
            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            
            builder.Register<GameplayStateMachine>(container =>
            {
                var creator = container.Resolve<GameplayStateMachineCreator>();
                return creator.Create();
            }, Lifetime.Singleton).As<ITickable>();
            
            builder.Register<PositionsCreator>(Lifetime.Singleton);
            builder.Register<CharacterCreator>(Lifetime.Singleton);
            builder.Register<BotMoveCreator>(Lifetime.Singleton);
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