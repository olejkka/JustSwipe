using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.States;

namespace _Project.Scripts.Infrastructure.FSM
{
    public interface IGameplayStatesProvider
    {
        IReadOnlyList<IState> GetStates();
        Type GetStartState();
    }
}