using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
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
        private readonly CharacterCreator _creator;
        private readonly CharactersStorage _charactersStorage;
        private readonly PauseService _pauseService;
        
        
        public GameplayStatesProvider(
            SwipeInputHandler swipeInputHandler,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterSpawnController characterSpawnController,
            TurnService turnService,
            CharacterCreator creator,
            CharactersStorage charactersStorage,
            PauseService pauseService
            )
        {
            _swipeInputHandler = swipeInputHandler;
            _botMoveCreator = botMoveCreator;
            _charactersMover = charactersMover;
            _characterSpawnController = characterSpawnController;
            _turnService = turnService;
            _creator = creator;
            _charactersStorage = charactersStorage;
            _pauseService = pauseService;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<BotTurnState>(() => _turnService.PlayerMoveFinished),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any()),
                },
                _swipeInputHandler,
                _charactersMover,
                _characterSpawnController,
                _turnService,
                _creator
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<PlayerTurnState>(() => _turnService.BotMoveFinished),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any())
                },
                _botMoveCreator,
                _charactersMover,
                _turnService,
                _creator,
                _charactersStorage
            );
            
            var endGameState = new EndGameState(
                new ITransition[]
                {
                },
                _pauseService
            );
            
            return new IState[] 
            {
                playerTurnState,
                botTurnState,
                endGameState
            };
        }
        
        public Type GetStartState() => typeof(PlayerTurnState);
    }
}