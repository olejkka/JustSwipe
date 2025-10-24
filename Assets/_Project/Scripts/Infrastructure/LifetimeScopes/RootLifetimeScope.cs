using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.Initializers;
using _Project.Scripts.InputHandlers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            
        }
    }
}