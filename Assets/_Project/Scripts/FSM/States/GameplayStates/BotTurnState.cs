using System.Collections.Generic;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class BotTurnState : State
    {
        public BotTurnState(IReadOnlyList<ITransition> transitions) : base(transitions) { }
        public override void Enter() { }
        public override void Exit() { }
        public override void Update() { }
    }
}