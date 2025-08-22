using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class EndGameState : State
    {
        public EndGameState(IReadOnlyList<ITransition> transitions) : base(transitions)
        {
        }

        public override void Enter()
        {
            // Debug.Log("[EndGameState] Entering EndGameState");
        }

        public override void Exit()
        {
            // Debug.Log("[EndGameState] Exiting EndGameState");
        }

        public override void Update()
        {
        }
    }
}