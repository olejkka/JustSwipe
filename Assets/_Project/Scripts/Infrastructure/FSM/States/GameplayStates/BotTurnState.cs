using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class BotTurnState : State
    {
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly TurnService _turnService;
        
        private bool _handled;


        public BotTurnState(
            IReadOnlyList<ITransition> transitions,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            TurnService turnService
        ) : base(transitions)
        {
            _botMoveCreator = botMoveCreator;
            _charactersMover = charactersMover;
            _turnService = turnService;
        }

        public override void Enter()
        {
            // Debug.Log("[BotTurnState] Entering Bot Turn State");
            
            _charactersMover.OnMove += OnBotCharactersMoved;
            
            _handled = false;
            _turnService.BotMoveFinished = false;
            
            var direction = _botMoveCreator.GenerateRandomDirection();
            _charactersMover.Move(direction, Team.Bot);
        }

        public override void Exit()
        {
            _charactersMover.OnMove -= OnBotCharactersMoved;
            
            _turnService.BotMoveFinished = false;
            
            // Debug.Log("[BotTurnState] Exiting Bot Turn State");
        }
        public override void Update() { }
        
        private void OnBotCharactersMoved()
        {
            if (_handled) 
                return;
            
            _handled = true;
            _turnService.BotMoveFinished = true;
        }
    }
}