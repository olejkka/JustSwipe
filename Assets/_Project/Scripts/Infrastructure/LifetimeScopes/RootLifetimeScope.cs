using _Project.Scripts.Characters;
using _Project.Scripts.Economy;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private TilesGenerationConfig _tilesGenerationConfig;
        [SerializeField] private CharactersPrefabsConfig _charactersPrefabsConfig;
        [SerializeField] private CharacterStatsConfig _characterStatsConfig;
        [SerializeField] private BotsDeathRewardsConfig _botsDeathRewardsConfig;
        [SerializeField] private CharacterConfig[] _characterConfigs;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerMoney>(Lifetime.Singleton);
            
            RegisterConfigs(builder);
        }
        
        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGenerationConfig);
            builder.RegisterInstance(_characterStatsConfig);
            builder.RegisterInstance(_charactersPrefabsConfig);
            builder.RegisterInstance(_botsDeathRewardsConfig);
            builder.RegisterInstance(_characterConfigs);
        }
    }
}