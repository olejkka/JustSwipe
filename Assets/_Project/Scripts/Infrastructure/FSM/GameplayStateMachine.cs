using System;
using System.Collections.Generic;

namespace _Project.Scripts.FSM
{
    public class GameplayStateMachine : FiniteStateMachine
    {
        public GameplayStateMachine(Dictionary<Type, IState> states) : base(states)
        {
        }
    }
}