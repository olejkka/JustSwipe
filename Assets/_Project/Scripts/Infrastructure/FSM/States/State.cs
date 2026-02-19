using System;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.FSM.States
{
    public abstract class State : IState
    {
        private readonly IReadOnlyList<ITransition> _transitions;

        protected State(IReadOnlyList<ITransition> transitions)
        {
            _transitions = transitions;
        }

        public void Enter()
        {
            foreach (var transition in _transitions)
                transition.Reset();

            OnEnter();
        }

        protected abstract void OnEnter();

        public abstract void Exit();

        public abstract void Update();

        public bool TryGetNextState(out Type type)
        {
            foreach (var transition in _transitions)
            {
                if (transition.CanTransit())
                {
                    type = transition.NextState;
                    return true;
                }
            }

            type = null;
            return false;
        }
    }
}