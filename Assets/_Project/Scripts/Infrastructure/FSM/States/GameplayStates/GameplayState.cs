using System.Collections.Generic;
using _Project.Scripts.Infrastructure.GameplayPhases;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class GameplayState : State
    {
        private readonly PhaseHandler _phaseHandler;

        
        public GameplayState(
            IReadOnlyList<ITransition> transitions,
            PhaseHandler phaseHandler,
            PauseService pauseService
        ) : base(transitions)
        {
            _phaseHandler = phaseHandler;
        }

        public override void Enter()
        {
            Time.timeScale = 1f;
            
            _phaseHandler.Start();
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}