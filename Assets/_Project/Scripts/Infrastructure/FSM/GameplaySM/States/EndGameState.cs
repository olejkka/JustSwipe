using System.Collections.Generic;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.Core;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States
{
    public class EndGameState : State
    {
        private readonly EventBus.EventBus _eventBus;

        
        public EndGameState(
            IReadOnlyList<ITransition> transitions,
            EventBus.EventBus eventBus) : base(transitions)
        {
            _eventBus = eventBus;
        }

        protected override void OnEnter()
        {
            _eventBus.Publish(new ShowGameplayStatisticEvent());
        }

        protected override void OnExit() { }

        public override void Update() { }
    }
}