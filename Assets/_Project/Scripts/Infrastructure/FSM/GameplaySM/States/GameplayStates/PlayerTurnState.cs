using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        private readonly EventBus _eventBus;
        private readonly CharactersMover _charactersMover;
        

        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions, 
            EventBus eventBus,
            CharactersMover charactersMover
        ) : base(transitions)
        {
            _eventBus = eventBus;
            _charactersMover = charactersMover;
        }

        protected override void OnEnter()
        {
            _eventBus.Subscribe<SwipeEvent>(OnSwipe);
        }

        public override void Exit()
        {
            _eventBus.Unsubscribe<SwipeEvent>(OnSwipe);
        }

        public override void Update() { }

        private void OnSwipe(SwipeEvent e)
        {
            _charactersMover.Move(e.Direction, Team.Player);
        }
    }
}