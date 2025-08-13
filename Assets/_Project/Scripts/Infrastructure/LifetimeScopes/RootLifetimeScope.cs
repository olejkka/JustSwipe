using _Project.Scripts.Creators;
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
            
            RegisterInputHandlers(builder);
        }
        
        private void RegisterInputHandlers(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<KeyboardInputHandler>();
            builder.RegisterComponentInHierarchy<SwipeInputHandler>();
            builder.RegisterComponentInHierarchy<BotInputHandler>();

            builder.Register<IInputHandler>(
                resolver => Application.isMobilePlatform
                    ? resolver.Resolve<SwipeInputHandler>()
                    : resolver.Resolve<KeyboardInputHandler>(),
                Lifetime.Singleton
            );
        }
    }
}