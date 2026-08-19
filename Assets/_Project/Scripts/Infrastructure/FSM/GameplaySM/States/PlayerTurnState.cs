using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Movement;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.Core;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States
{
    public class PlayerTurnState : State
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly CharactersTurnOrchestrator _charactersTurnOrchestrator;
        

        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions, 
            EventBus.EventBus eventBus,
            CharactersTurnOrchestrator charactersTurnOrchestrator) : base(transitions)
        {
            _eventBus = eventBus;
            _charactersTurnOrchestrator = charactersTurnOrchestrator;
        }

        protected override void OnEnter()
        {
            _eventBus.Subscribe<SwipeEvent>(OnSwipe);
        }

        protected override void OnExit()
        {
            _eventBus.Publish(new TurnEndedEvent(Team.Player));
            _eventBus.Unsubscribe<SwipeEvent>(OnSwipe);
        }

        public override void Update() { }

        private void OnSwipe(SwipeEvent e)
        {
            _charactersTurnOrchestrator.Execute(e.Direction, Team.Player);
        }
    }
}