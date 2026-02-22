using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
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