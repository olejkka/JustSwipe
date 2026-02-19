using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        private const float SpawnChance = 0.25f;
        
        private readonly EventBus _eventBus;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterCreator _creator;

        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions, 
            EventBus eventBus,
            CharactersMover charactersMover,
            CharacterCreator characterCreator
        ) : base(transitions)
        {
            _eventBus = eventBus;
            _charactersMover = charactersMover;
            _creator = characterCreator;
        }

        protected override void OnEnter()
        {
            _eventBus.Subscribe<SwipeEvent>(OnSwipe);
        }

        public override void Exit()
        {
            _eventBus.Unsubscribe<SwipeEvent>(OnSwipe);
            
            if (Random.value < SpawnChance)
                _creator.CreateOnRandomPos(CharacterType.Bot_1);
        }

        public override void Update() { }

        private void OnSwipe(SwipeEvent e)
        {
            _charactersMover.Move(e.Direction, Team.Player);
        }
    }
}