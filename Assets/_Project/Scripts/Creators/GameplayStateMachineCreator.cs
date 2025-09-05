using System;
using System.Collections.Generic;
using _Project.Scripts.FSM;
using _Project.Scripts.Infrastructure.FSM;

namespace _Project.Scripts.Creators
{
    public class GameplayStateMachineCreator
    {
        private readonly IGameplayStatesProvider _provider;

        public GameplayStateMachineCreator(IGameplayStatesProvider provider)
        {
            _provider = provider;
        }

        public GameplayStateMachine Create()
        {
            var list = _provider.GetStates() ?? Array.Empty<IState>();
            var dictionary = new Dictionary<Type, IState>();

            foreach (var state in list)
                dictionary[state.GetType()] = state;

            var fsm = new GameplayStateMachine(dictionary);

            fsm.EnterState(_provider.GetStartState());

            return fsm;
        }
    }
}