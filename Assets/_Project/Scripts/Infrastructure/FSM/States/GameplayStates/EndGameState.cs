using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class EndGameState : State
    {
        private readonly PauseService _pauseService;

        public EndGameState(
            IReadOnlyList<ITransition> transitions,
            PauseService pauseService
        ) : base(transitions)
        {
            _pauseService = pauseService;
        }

        protected override void OnEnter()
        {
            Debug.Log("[EndGameState] Entering EndGameState");
            _pauseService.TogglePause();
        }

        public override void Exit() { }

        public override void Update() { }
    }
}