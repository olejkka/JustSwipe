using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class PauseState : State
    {
        public PauseState(IReadOnlyList<ITransition> transitions) : base(transitions) { }

        public override void Enter()
        {
            // Debug.Log("[PauseState] Entering PauseState");

        }

        public override void Exit()
        {
            // Debug.Log("[PauseState] Exiting PauseState");

        }
        public override void Update() { }
    }
}