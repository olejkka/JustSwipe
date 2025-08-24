using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.Economy;
using _Project.Scripts.FSM;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.GameplayPhases;
using _Project.Scripts.Infrastructure.Initializers;
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
        [SerializeField] private BotsDeathRewardsContig _botsDeathRewardsContig;
        [SerializeField] private TileInstantiator _tileInstantiator;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayInitializer>();
            
            builder.Register<CharactersMover>(Lifetime.Singleton);
            builder.Register<CharactersDeathHandler>(Lifetime.Singleton);
            builder.Register<BotDeathRewardService>(Lifetime.Singleton);

            builder.Register<GameplayStateMachineCreator>(Lifetime.Singleton);
            builder.Register<IGameplayStatesProvider, GameplayStatesProvider>(Lifetime.Singleton);
            builder.Register<TurnService>(Lifetime.Singleton);

            builder.Register<PhaseHandler>(Lifetime.Singleton).As<IStartable>().AsSelf();
            builder.Register<InputStorage>(Lifetime.Singleton);
            builder.Register<InputReadingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            builder.Register<CharactersMovingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();

            RegisterInputHandlers(builder);
            RegisterConfigs(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
        }

        private void RegisterInputHandlers(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
            builder.RegisterComponentInHierarchy<BotInputHandler>();
            builder.Register<PlayerInputHandler>(Lifetime.Singleton);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);
            builder.RegisterInstance(_botsDeathRewardsContig);
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