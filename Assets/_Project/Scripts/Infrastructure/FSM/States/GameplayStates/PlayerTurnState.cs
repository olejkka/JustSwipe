using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.InputHandlers;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        private const float SpawnChance = 0.25f;
        
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterSpawnController _characterSpawnController;
        private readonly TurnService _turnService;
        private readonly CharacterCreator _creator;
        
        private bool _handled;
        
        
        
        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions, 
            SwipeInputHandler swipeInputHandler,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService,
            CharacterCreator characterCreator
            ) : base(transitions)
        {
            _swipeInputHandler = swipeInputHandler;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turnService = turnService;
            _creator = characterCreator;
        }

        public override void Enter()
        {
            // Debug.Log("[PlayerTurnState] Entering Player Turn State");
            
            _swipeInputHandler.OnPressed += _charactersMover.Move;
            _swipeInputHandler.OnPressed += _characterSpawnController.HandleInput;
            _charactersMover.OnMove += OnPlayerCharactersMoved;
            
            _handled = false;
            _turnService.PlayerMoveFinished = false;
        }

        public override void Exit()
        {
            _swipeInputHandler.OnPressed -= _charactersMover.Move;
            _swipeInputHandler.OnPressed -= _characterSpawnController.HandleInput;
            _charactersMover.OnMove -= OnPlayerCharactersMoved;
            
            _turnService.PlayerMoveFinished = false;
            
            if (Random.value < SpawnChance)
                _creator.CreateOnRandomPos(CharacterType.Bot_1);
            
            // Debug.Log("[PlayerTurnState] Exiting Player Turn State");
        }
        public override void Update() { }
        
        private void OnPlayerCharactersMoved()
        {
            if (_handled)
                return;
            
            _handled = true;
            _turnService.PlayerMoveFinished = true;
        }
    }
}