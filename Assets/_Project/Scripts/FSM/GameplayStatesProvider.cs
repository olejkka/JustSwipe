using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.FSM.States.GameplayStates;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotInputHandler _botInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterSpawnController _characterSpawnController;
        private readonly ITurnService _turns;

        
        public GameplayStatesProvider(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            ITurnService turns
            )
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turns = turns;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<BotTurnState>(() => _turns.PlayerMoveFinished)
                },
                _keyboardInputHandler,
                _swipeInputHandler,
                _charactersMover,
                _characterSpawnController,
                _turns
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<PlayerTurnState>(() => _turns.BotMoveFinished)
                },
                _botInputHandler,
                _charactersMover,
                _turns
            );
            
            return new IState[] 
            {
                playerTurnState, 
                botTurnState, 
                new PauseState(Array.Empty<ITransition>()), 
                new EndGameState(Array.Empty<ITransition>())
            };
        }
        
        public Type GetStartState() => typeof(PlayerTurnState);
    }
}