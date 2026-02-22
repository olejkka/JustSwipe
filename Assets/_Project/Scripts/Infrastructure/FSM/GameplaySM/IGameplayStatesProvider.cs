using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
{
    public interface IGameplayStatesProvider
    {
        IReadOnlyList<IState> GetStates();
        Type GetStartState();
    }
}