using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates
{
    public class EndGameState : State
    {
        private readonly EventBus _eventBus;

        
        public EndGameState(
            IReadOnlyList<ITransition> transitions,
            EventBus eventBus
        ) : base(transitions)
        {
            _eventBus = eventBus;
        }

        protected override void OnEnter()
        {
            _eventBus.Publish(new ReturnToMenuEvent());
        }

        public override void Exit() { }

        public override void Update() { }
    }
}