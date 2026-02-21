using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.States;
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