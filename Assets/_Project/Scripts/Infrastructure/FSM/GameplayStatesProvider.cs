using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.FSM;
using _Project.Scripts.FSM.States.GameplayStates;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotInputHandler _botInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterSpawnController _characterSpawnController;
        private readonly TurnService _turnService;

        
        public GameplayStatesProvider(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService
            )
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turnService = turnService;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<BotTurnState>(() => _turnService.PlayerMoveFinished)
                },
                _keyboardInputHandler,
                _swipeInputHandler,
                _charactersMover,
                _characterSpawnController,
                _turnService
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<PlayerTurnState>(() => _turnService.BotMoveFinished)
                },
                _botInputHandler,
                _charactersMover,
                _turnService
            );
            
            return new IState[] 
            {
                playerTurnState,
                botTurnState,
                new EndGameState(Array.Empty<ITransition>())
            };
        }
        
        public Type GetStartState() => typeof(PlayerTurnState);
    }
}