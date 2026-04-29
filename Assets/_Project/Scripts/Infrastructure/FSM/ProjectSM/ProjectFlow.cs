using System;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.ProjectSM.States;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM
{
    public class ProjectFlow : IStartable, IDisposable
    {
        private readonly ProjectStateMachine _stateMachine;
        private readonly EventBus.EventBus _eventBus;

        public ProjectFlow(ProjectStateMachine stateMachine, EventBus.EventBus eventBus)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
        }

        public async void Start()
        {
            _eventBus.Subscribe<PlayClickedEvent>(OnPlayClicked);
            _eventBus.Subscribe<ReturnToMenuRequestedEvent>(OnReturnToMenu);

            await _stateMachine.EnterState<InitializationState>();
            await _stateMachine.EnterState<MenuState>();
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<PlayClickedEvent>(OnPlayClicked);
            _eventBus.Unsubscribe<ReturnToMenuRequestedEvent>(OnReturnToMenu);
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