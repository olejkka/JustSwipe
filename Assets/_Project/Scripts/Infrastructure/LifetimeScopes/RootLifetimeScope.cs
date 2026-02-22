using _Project.Scripts.Infrastructure.FSM.ProjectSM;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayConfigs _gameplayConfigs;

        protected override void Configure(IContainerBuilder builder)
        {
            _gameplayConfigs.RegisterAll(builder);

            builder.Register<EventBus>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.Register<ProjectStatesProvider>(Lifetime.Singleton);
            builder.Register<ProjectStateMachine>(container =>
            {
                var provider = container.Resolve<ProjectStatesProvider>();
                return new ProjectStateMachine(provider.CreateStates());
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<ProjectFlow>();
        }
    }
}