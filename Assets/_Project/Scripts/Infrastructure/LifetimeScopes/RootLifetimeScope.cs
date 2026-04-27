using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Audio;
using _Project.Scripts.Infrastructure.FSM.ProjectSM;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayConfigs _gameplayConfigs;
        [SerializeField] private AudioService _audioService;

        
        protected override void Configure(IContainerBuilder builder)
        {
            _gameplayConfigs.RegisterAll(builder);
            
            builder.RegisterComponent(_audioService);

            builder.Register<EventBus>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<AudioEventHandler>();
            builder.RegisterEntryPoint<ApplicationQuitHandler>();
            
            builder.Register<ProjectStatesProvider>(Lifetime.Singleton);
            builder.Register<PauseService>(Lifetime.Singleton);
            
            builder.Register<ProjectStateMachine>(container =>
            {
                var provider = container.Resolve<ProjectStatesProvider>();
                return new ProjectStateMachine(provider.CreateStates());
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<ProjectFlow>();
        }
    }
}