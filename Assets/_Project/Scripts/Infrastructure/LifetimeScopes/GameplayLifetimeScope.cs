using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.Economy;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure.GameplayPhases;
using _Project.Scripts.Infrastructure.GameplayPhases.Phases;
using _Project.Scripts.Infrastructure.Initializers;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.Instantiators;
using _Project.Scripts.Tiles;
using _Project.Scripts.UI.CharacterHP;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TileInstantiator _tileInstantiator;
        [SerializeField] private CharacterConfig[] _characterConfigs;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
            
            builder.RegisterInstance(_characterConfigs);

            builder.Register<CharactersMovementOrchestrator>(Lifetime.Singleton);
            builder.Register<CharactersCombatHandler>(Lifetime.Singleton);
            builder.Register<CharactersMovementHandler>(Lifetime.Singleton);
            builder.Register<CharactersDeathHandler>(Lifetime.Singleton);
            builder.Register<DeathRewardService>(Lifetime.Singleton);
            builder.Register<CharactersPositionValidator>(Lifetime.Singleton);

            RegisterInputHandlers(builder);
            RegisterCreators(builder);
            RegisterInstantiators(builder);
            RegisterStorages(builder);
            RegisterPhases(builder);
        }

        private void RegisterInputHandlers(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
            builder.RegisterComponentInHierarchy<BotInputHandler>();
            builder.Register<PlayerInputHandler>(Lifetime.Singleton);
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
            builder.RegisterComponentInHierarchy<CharacterHPViewInstantiator>();
        }

        private void RegisterStorages(IContainerBuilder builder)
        {
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
            builder.Register<CharactersViewsStorage>(Lifetime.Singleton);
        }

        private void RegisterPhases(IContainerBuilder builder)
        {
            builder.Register<PhaseHandler>(Lifetime.Singleton).As<IStartable>().AsSelf();
            builder.Register<InputStorage>(Lifetime.Singleton);

            builder.Register<InputReadingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            builder.Register<EnemySpawnPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            builder.Register<CharactersMovingPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
            builder.Register<DefeatPhase>(Lifetime.Singleton).As<Phase>().AsSelf();
        }
    }
}