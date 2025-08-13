using System;
using System.Collections.Generic;
using _Project.Scripts.FSM.States.GameplayStates;

namespace _Project.Scripts.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        public IReadOnlyList<IState> GetStates()
        {
            return new IState[]
            {
                new PlayerTurnState(Array.Empty<ITransition>()),
                new BotTurnState(Array.Empty<ITransition>()),
                new PauseState(Array.Empty<ITransition>())
            };
        }
        
        public Type GetStartState()
        {
            return typeof(PlayerTurnState);
        }
    }
}