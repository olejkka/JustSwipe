using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.ScriptableObjects;

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
        private readonly CharacterStatsConfig _characterStatsConfig;


        public GameplayStatesProvider(
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMover charactersMover,
            PlayerInputHandler playerInputHandler,
            TurnService turnService,
            PauseService pauseService,
            CharactersStorage charactersStorage,
            CharacterCreator characterCreator,
            CharacterStatsConfig characterStatsConfig
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
            _characterStatsConfig = characterStatsConfig;
        }

        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused),
                    new TransitionTo<BotTurnState>(() => _turnService.PlayerMoveFinished),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any())
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
                    new TransitionTo<PlayerTurnState>(() => _turnService.BotMoveFinished),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any())
                },
                _botInputHandler,
                _charactersMover,
                _turnService,
                _pauseService,
                _charactersStorage,
                _characterCreator,
                _characterStatsConfig
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