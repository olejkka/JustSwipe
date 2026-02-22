using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Infrastructure.FSM.ProjectSM.States;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM
{
    public class ProjectStateMachine
    {
        private readonly Dictionary<Type, IProjectState> _states;
        private IProjectState _currentState;

        public ProjectStateMachine(Dictionary<Type, IProjectState> states)
        {
            _states = states;
        }

        public async Task EnterState<TState>() where TState : IProjectState
        {
            if (_currentState != null)
                await _currentState.Exit();

            _currentState = _states[typeof(TState)];
            await _currentState.Enter();
        }
    }
}