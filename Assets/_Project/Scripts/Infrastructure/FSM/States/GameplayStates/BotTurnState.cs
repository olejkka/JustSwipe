using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.FSM;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class BotTurnState : State
    {
        private readonly BotInputHandler _botInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly PauseService _pauseService;
        private readonly TurnService _turnService;

        private bool _handled;


        public BotTurnState(
            IReadOnlyList<ITransition> transitions,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            TurnService turnService,
            PauseService pauseService
        ) : base(transitions)
        {
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _turnService = turnService;
            _pauseService = pauseService;
        }

        public override void Enter()
        {
            // Debug.Log("[BotTurnState] Entering Bot Turn State");

            _pauseService.ResumeToPlayer = false;


            _botInputHandler.OnPressed += _charactersMover.Move;
            _charactersMover.OnMove += OnBotCharactersMoved;

            _handled = false;
            _turnService.BotMoveFinished = false;
        }

        public override void Exit()
        {
            _botInputHandler.OnPressed -= _charactersMover.Move;
            _charactersMover.OnMove -= OnBotCharactersMoved;

            _turnService.BotMoveFinished = false;

            // Debug.Log("[BotTurnState] Exiting Bot Turn State");
        }

        public override void Update()
        {
        }

        private void OnBotCharactersMoved()
        {
            if (_handled)
                return;

            _handled = true;
            _turnService.BotMoveFinished = true;
        }
    }
}