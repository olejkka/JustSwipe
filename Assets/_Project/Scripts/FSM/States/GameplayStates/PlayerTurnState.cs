using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        public PlayerTurnState(IReadOnlyList<ITransition> transitions) : base(transitions) { }

        public override void Enter()
        {
            Debug.Log("[PlayerTurnState] Entering Player Turn State");
        }
        public override void Exit() { }
        public override void Update() { }
    }
}