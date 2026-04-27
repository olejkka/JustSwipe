using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
{
    public abstract class FiniteStateMachine
    {
        private IState _currentState;
        private Dictionary<Type, IState> _states;

        public FiniteStateMachine(Dictionary<Type, IState> states)
        {
            _states = states;
        }

        public void EnterState<TState>()
        {
            EnterState(typeof(TState));
        }

        public void EnterState(Type type)
        {
            if (_states.TryGetValue(type, out var state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }

        public void UpdateState()
        {
            if (_currentState == null)
                return;

            _currentState.Update();

            if (_currentState.TryGetNextState(out Type type))
                EnterState(type);
        }
        
        public void Stop()
        {
            _currentState?.Exit();
            _currentState = null;
        }
    }
}