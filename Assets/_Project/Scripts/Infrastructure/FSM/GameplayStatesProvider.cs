using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.InputHandlers;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly BotInputHandler _botInputHandler;
        private readonly CharactersMover _charactersMover;
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly PauseService _pauseService;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly TurnService _turnService;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharacterCreator _characterCreator;


        public GameplayStatesProvider(
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            PlayerInputHandler playerInputHandler,
            TurnService turnService,
            PauseService pauseService,
            CharactersStorage charactersStorage,
            CharacterCreator characterCreator
        )
        {
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _charactersMover = charactersMover;
            _playerInputHandler = playerInputHandler;
            _turnService = turnService;
            _pauseService = pauseService;
            _charactersStorage = charactersStorage;
            _characterCreator = characterCreator;
        }

        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused),
                    new TransitionTo<BotTurnState>(() => _turnService.PlayerMoveFinished)
                    // new TransitionTo<EndGameState>(() => _turnService.PlayerMoveFinished)
                },
                _swipeInputHandler,
                _charactersMover,
                _playerInputHandler,
                _turnService,
                _pauseService
            );

            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused),
                    new TransitionTo<PlayerTurnState>(() => _turnService.BotMoveFinished)
                    // new TransitionTo<EndGameState>(() => _turnService.PlayerMoveFinished)
                },
                _botInputHandler,
                _charactersMover,
                _turnService,
                _pauseService,
                _charactersStorage,
                _characterCreator
            );

            var pauseState = new PauseState(
                new ITransition[]
                {
                    new TransitionTo<PlayerTurnState>(() => !_pauseService.IsPaused && _pauseService.ResumeToPlayer),
                    new TransitionTo<BotTurnState>(() => !_pauseService.IsPaused && !_pauseService.ResumeToPlayer)
                }
            );

            var endGameState = new EndGameState(
                new ITransition[]
                {
                }
            );

            return new IState[]
            {
                playerTurnState,
                botTurnState,
                pauseState,
                endGameState
            };
        }

        public Type GetStartState()
        {
            return typeof(PlayerTurnState);
        }
    }
}