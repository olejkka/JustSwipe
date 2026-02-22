using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.FSM.States;

namespace _Project.Scripts.Creators
{
    public class GameplayStateMachineCreator
    {
        private readonly GameplayStatesProvider _provider;

        public GameplayStateMachineCreator(GameplayStatesProvider provider)
        {
            _provider = provider;
        }

        public GameplayStateMachine Create()
        {
            var list = _provider.GetStates() ?? Array.Empty<IState>();
            var dictionary = new Dictionary<Type, IState>();
        
            foreach (var state in list)
                dictionary[state.GetType()] = state;

            return new GameplayStateMachine(dictionary);
        }
    }
}