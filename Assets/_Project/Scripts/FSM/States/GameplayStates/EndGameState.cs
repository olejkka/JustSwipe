using System.Collections.Generic;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class EndGameState : State
    {
        public EndGameState(IReadOnlyList<ITransition> transitions) : base(transitions) { }
        public override void Enter() { }
        public override void Exit() { }
        public override void Update() { }
    }
}