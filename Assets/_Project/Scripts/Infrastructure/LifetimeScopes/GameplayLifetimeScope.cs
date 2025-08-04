using _Project.Scripts.Characters;
using _Project.Scripts.Factories;
using _Project.Scripts.Generators;
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
        [SerializeField] private TileFactory tileFactory;


        protected override void Configure(IContainerBuilder builder)
        {
            // EntryPoints
            builder.RegisterEntryPoint<EntryPoint>();

            // Configs
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);

            // Generators
            builder.Register<PositionsGenerator>(Lifetime.Singleton);
            builder.Register<CharacterPositionGenerator>(Lifetime.Singleton);
            
            // Factories
            builder.RegisterInstance(tileFactory);
            builder.RegisterComponentInHierarchy<CharacterFactory>();
            
            // Storages
            builder.Register<PositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
        }
    }
}