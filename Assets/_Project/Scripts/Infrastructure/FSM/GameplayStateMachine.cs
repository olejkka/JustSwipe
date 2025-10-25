using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStateMachine : FiniteStateMachine, ITickable
    {
        public GameplayStateMachine(Dictionary<Type, IState> states) : base(states)
        {
        }
        
        public void Tick()
        {
            UpdateState();
        }
    }
}