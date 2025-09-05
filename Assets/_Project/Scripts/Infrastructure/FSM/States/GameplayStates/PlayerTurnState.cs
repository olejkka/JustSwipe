using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.FSM;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class PlayerTurnState : State
    {
        private readonly CharactersMovementOrchestrator _charactersMovementOrchestrator;
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly PauseService _pauseService;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly TurnService _turnService;

        private bool _handled;


        public PlayerTurnState(
            IReadOnlyList<ITransition> transitions,
            SwipeInputHandler swipeInputHandler,
            CharactersMovementOrchestrator charactersMovementOrchestrator,
            PlayerInputHandler playerInputHandler,
            TurnService turnService,
            PauseService pauseService
        ) : base(transitions)
        {
            _swipeInputHandler = swipeInputHandler;
            _charactersMovementOrchestrator = charactersMovementOrchestrator;
            _playerInputHandler = playerInputHandler;
            _turnService = turnService;
            _pauseService = pauseService;
        }

        public override void Enter()
        {
           _pauseService.ResumeToPlayer = true;

            // _swipeInputHandler.OnPressed += _charactersMovementOrchestrator.ProcessTurn;
            // _swipeInputHandler.OnPressed += _playerInputHandler.Handle;
            // _charactersMovementOrchestrator.OnTurnCompleted += OnPlayerCharactersMoved;

            _handled = false;
            _turnService.PlayerMoveFinished = false;
        }

        public override void Exit()
        {
            // _swipeInputHandler.OnPressed -= _charactersMovementOrchestrator.ProcessTurn;
            // _swipeInputHandler.OnPressed -= _playerInputHandler.Handle;
            // _charactersMovementOrchestrator.OnTurnCompleted -= OnPlayerCharactersMoved;
            //
            // _turnService.PlayerMoveFinished = false;
        }

        public override void Update()
        {
        }

        private void OnPlayerCharactersMoved()
        {
            if (_handled)
                return;

            _handled = true;
            _turnService.PlayerMoveFinished = true;
        }
    }
}