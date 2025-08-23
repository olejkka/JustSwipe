using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class EndGameState : State
    {
        public event Action OnEndGame;
            
            
        public EndGameState(IReadOnlyList<ITransition> transitions) : base(transitions)
        {
        }

        public override void Enter()
        {
            Time.timeScale = 0f;
            OnEndGame?.Invoke();
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}