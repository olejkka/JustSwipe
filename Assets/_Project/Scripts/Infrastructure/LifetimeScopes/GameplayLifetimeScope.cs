using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using _Project.Scripts.UI;
using _Project.Scripts.UI.CharacterCaseUI;
using _Project.Scripts.UI.CharacterPurchaseCase;
using _Project.Scripts.UI.MoneyUI;
using _Project.Scripts.UI.SettingsButton;
using _Project.Scripts.Wallet;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TileInstantiator _tileInstantiator;
        [SerializeField] private CharacterViewInstantiator _characterViewInstantiator;

        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInstantiators(builder);
            RegisterCreators(builder);
            RegisterStorages(builder);
    
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
    
            builder.Register<Money>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);

            builder.Register<IGameplayStatesProvider, GameplayStatesProvider>(Lifetime.Singleton);

            builder.RegisterEntryPoint<CharacterDeathHandler>();
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }

        private void RegisterCreators(IContainerBuilder builder)
        {
            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            
            builder.Register<GameplayStateMachine>(container =>
            {
                var creator = container.Resolve<GameplayStateMachineCreator>();
                return creator.Create();
            }, Lifetime.Singleton).AsSelf().As<ITickable>();
            
            builder.Register<PositionsCreator>(Lifetime.Singleton);
            builder.Register<CharacterCreator>(Lifetime.Singleton);
            builder.Register<BotMoveCreator>(Lifetime.Singleton);
        }

        private void RegisterInstantiators(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tileInstantiator);
            builder.RegisterComponent(_characterViewInstantiator);
        }

        private void RegisterStorages(IContainerBuilder builder)
        {
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<CharactersViewsStorage>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}