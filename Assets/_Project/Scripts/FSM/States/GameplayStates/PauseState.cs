using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class PauseState : State
    {
        public PauseState(IReadOnlyList<ITransition> transitions) : base(transitions) { }

        public override void Enter()
        {
            
        }
        public override void Exit() { }
        public override void Update() { }
    }
}