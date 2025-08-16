using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.FSM.States.GameplayStates
{
    public class BotTurnState : State
    {
        private readonly BotInputHandler _botInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly ITurnService _turns;
        private bool _handled;


        public BotTurnState(
            IReadOnlyList<ITransition> transitions,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            ITurnService turns
        ) : base(transitions)
        {
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _turns = turns;
        }

        public override void Enter()
        {
            // Debug.Log("[BotTurnState] Entering Bot Turn State");
            
            _botInputHandler.OnPressed += _charactersMover.Move;
            _charactersMover.OnMove += OnBotCharactersMoved;
            
            _handled = false;
            _turns.BotMoveFinished = false;
        }

        public override void Exit()
        {
            _botInputHandler.OnPressed -= _charactersMover.Move;
            _charactersMover.OnMove -= OnBotCharactersMoved;
            
            _turns.BotMoveFinished = false;
            
            // Debug.Log("[BotTurnState] Exiting Bot Turn State");
        }
        public override void Update() { }
        
        private void OnBotCharactersMoved()
        {
            if (_handled) 
                return;
            
            _handled = true;
            _turns.BotMoveFinished = true;
        }
    }
}