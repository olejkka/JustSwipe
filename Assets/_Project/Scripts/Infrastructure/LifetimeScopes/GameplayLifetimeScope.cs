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
        [SerializeField] private TilesGenerationSettingsConfig _tilesGenerationSettingsConfig;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        [SerializeField] private TileFactory _tileFactory;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EntryPoint>();

            builder.RegisterInstance(_tilesGenerationSettingsConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);

            builder.Register<TilesPositionsGenerator>(Lifetime.Singleton);
            builder.Register<CharacterPositionGenerator>(Lifetime.Singleton);
            
            builder.RegisterInstance(_tileFactory);
            builder.RegisterComponentInHierarchy<CharacterFactory>();
            
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
        }
    }
}