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
        private readonly PauseService _pauseService;

        
        public GameplayStatesProvider(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService,
            PauseService pauseService
            )
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turnService = turnService;
            _pauseService = pauseService;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused),
                    new TransitionTo<BotTurnState>(() => _turnService.PlayerMoveFinished)
                },
                _keyboardInputHandler,
                _swipeInputHandler,
                _charactersMover,
                _characterSpawnController,
                _turnService,
                _pauseService
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused),
                    new TransitionTo<PlayerTurnState>(() => _turnService.BotMoveFinished)
                },
                _botInputHandler,
                _charactersMover,
                _turnService,
                _pauseService
            );

            var pauseState = new PauseState(
                new ITransition[]
                {
                    new TransitionTo<PlayerTurnState>(() => !_pauseService.IsPaused && _pauseService.ResumeToPlayer),
                    new TransitionTo<BotTurnState>(() => !_pauseService.IsPaused && !_pauseService.ResumeToPlayer)
                }
            );
            
            return new IState[] 
            {
                playerTurnState,
                botTurnState,
                pauseState,
                new EndGameState(Array.Empty<ITransition>())
            };
        }
        
        public Type GetStartState() => typeof(PlayerTurnState);
    }
}