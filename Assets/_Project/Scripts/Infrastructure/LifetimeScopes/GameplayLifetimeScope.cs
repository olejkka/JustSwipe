using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using _Project.Scripts.UI;
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
        [SerializeField] private SwipeInputHandler _swipeInputHandler;
        [SerializeField] private TileInstantiator _tileInstantiator;
        [SerializeField] private CharacterViewInstantiator _characterViewInstantiator;


        protected override void Configure(IContainerBuilder builder)
        {
            //Input
            builder.RegisterComponent(_swipeInputHandler);
            
            //Instantiators
            builder.RegisterComponent(_tileInstantiator);
            builder.RegisterComponent(_characterViewInstantiator);

            //Creators
            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            builder.Register<GameplayStateMachine>(container =>
            {
                var creator = container.Resolve<GameplayStateMachineCreator>();
                return creator.Create();
            }, Lifetime.Singleton).AsSelf().As<ITickable>();
            builder.Register<PositionsCreator>(Lifetime.Singleton);
            builder.Register<CharacterCreator>(Lifetime.Singleton);
            builder.Register<BotMoveCreator>(Lifetime.Singleton);

            //Storages
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<CharactersViewsStorage>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            
            builder.Register<Money>(Lifetime.Singleton);
            builder.Register<CharactersMover>(Lifetime.Singleton);
            builder.Register<GameplayStatesProvider>(Lifetime.Singleton);
            builder.Register<PauseService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<CharacterDeathHandler>();
            builder.RegisterEntryPoint<KillRewardHandler>();
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}