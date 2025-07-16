using System.Collections.Generic;
using _Project.Scripts.Factories;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private TileFieldConfig _fieldConfig; 
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TileFactory>(Lifetime.Singleton)
                .As<ITileFactory>();

            builder.RegisterInstance(_fieldConfig)
                .As<TileFieldConfig>();
            
            builder.RegisterComponentInHierarchy<BoardGenerator>().AsSelf();
        }
    }
}