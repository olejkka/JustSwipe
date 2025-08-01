using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Factories;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.Generators;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationSettingsConfig _tilesGenerationSettingsConfig;
        [SerializeField] private TileFactory _tileFactory;
        [SerializeField] private CharacterFactory _characterFactory;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EntryPoint>();

            builder.RegisterInstance(_tileFactory);
            builder.RegisterInstance(_tilesGenerationSettingsConfig);
            builder.Register<TilesPositionsStorage>(Lifetime.Singleton);
            builder.Register<TilesPositionsGenerator>(Lifetime.Singleton);

            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);
            builder.RegisterInstance(_characterFactory);
            builder.Register<CharactersStorage>(Lifetime.Singleton);
            builder.Register<CharacterPositionGenerator>(Lifetime.Singleton);
        }
    }
}