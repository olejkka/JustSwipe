using System;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.ProjectSM.States;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM
{
    public class ProjectFlow : IStartable, IDisposable
    {
        private readonly ProjectStateMachine _stateMachine;
        private readonly EventBus.EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public ProjectFlow(ProjectStateMachine stateMachine, EventBus.EventBus eventBus)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
        }

        public async void Start()
        {
            _eventBus.SubscribeWithLifetime<PlayClickedEvent>(_lifetimeDefinition.Lifetime, OnPlayClicked);
            _eventBus.SubscribeWithLifetime<ReturnToMenuRequestedEvent>(_lifetimeDefinition.Lifetime, OnReturnToMenu);

            await _stateMachine.EnterState<InitializationState>();
            await _stateMachine.EnterState<MenuState>();
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        private async void OnPlayClicked(PlayClickedEvent e)
        {
            await _stateMachine.EnterState<GameplayState>();
        }

        private async void OnReturnToMenu(ReturnToMenuRequestedEvent e)
        {
            await _stateMachine.EnterState<MenuState>();
        }
    }
}