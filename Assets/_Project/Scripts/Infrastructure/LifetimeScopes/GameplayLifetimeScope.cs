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

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        
        [SerializeField] private TileInstantiator _tileInstantiator;
        
        // [SerializeField] private KeyboardInputHandler _keyboardInputHandler;
        // [SerializeField] private SwipeInputHandler swipeInputHandler;


        protected override void Configure(IContainerBuilder builder)
        {
            // EntryPoints
            builder.RegisterEntryPoint<EntryPoint>();
            
            // Input
            builder.RegisterComponentInHierarchy<KeyboardInputHandler>();
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();

            builder.Register<IInputHandler>(
                resolver => Application.isMobilePlatform
                    ? resolver.Resolve<SwipeInputHandler>()
                    : resolver.Resolve<KeyboardInputHandler>(),
                Lifetime.Singleton
            );
            // var provider = Application.isMobilePlatform
            //     ? (IInputHandler)swipeInputHandler
            //     : _keyboardInputHandler;
            //
            // builder.RegisterInstance(provider).As<IInputHandler>();

            // Configs
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);

            // Creators
            builder.Register<PositionsCreator>(Lifetime.Singleton);
            builder.Register<CharacterCreator>(Lifetime.Singleton);

            // Instantiators
            builder.RegisterInstance(_tileInstantiator);    
            builder.RegisterComponentInHierarchy<CharacterViewInstantiator>();
            
            builder.Register<CharactersMover>(Lifetime.Singleton);
            
            // Storages
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
        }
    }
}