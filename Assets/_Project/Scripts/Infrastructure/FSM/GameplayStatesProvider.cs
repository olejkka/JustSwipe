using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.FSM.States.GameplayStates;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterSpawnController _characterSpawnController;
        private readonly TurnService _turnService;

        
        public GameplayStatesProvider(
            SwipeInputHandler swipeInputHandler,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService
            )
        {
            _swipeInputHandler = swipeInputHandler;
            _botMoveCreator = botMoveCreator;
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
                _botMoveCreator,
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