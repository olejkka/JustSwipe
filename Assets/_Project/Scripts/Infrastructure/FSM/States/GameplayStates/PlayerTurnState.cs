using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.FSM;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterSpawnController _characterSpawnController;
        private readonly TurnService _turnService;
        
        private bool _handled;
        
        
        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions, 
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService
            ) : base(transitions)
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turnService = turnService;
        }

        public override void Enter()
        {
            // Debug.Log("[PlayerTurnState] Entering Player Turn State");
            
            _keyboardInputHandler.OnPressed += _charactersMover.Move;
            _swipeInputHandler.OnPressed += _charactersMover.Move;
            _keyboardInputHandler.OnPressed += _characterSpawnController.HandleInput;
            _swipeInputHandler.OnPressed += _characterSpawnController.HandleInput;
            _charactersMover.OnMove += OnPlayerCharactersMoved;
            
            _handled = false;
            _turnService.PlayerMoveFinished = false;
        }

        public override void Exit()
        {
            _keyboardInputHandler.OnPressed -= _charactersMover.Move;
            _swipeInputHandler.OnPressed -= _charactersMover.Move;
            _keyboardInputHandler.OnPressed -= _characterSpawnController.HandleInput;
            _swipeInputHandler.OnPressed -= _characterSpawnController.HandleInput;
            _charactersMover.OnMove -= OnPlayerCharactersMoved;
            
            _turnService.PlayerMoveFinished = false;
            
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