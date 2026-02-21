using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM.States;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly EventBus _eventBus;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterCreator _creator;
        private readonly CharactersStorage _charactersStorage;
        private readonly PauseService _pauseService;

        public GameplayStatesProvider(
            EventBus eventBus,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterCreator creator,
            CharactersStorage charactersStorage,
            PauseService pauseService
            )
        {
            _eventBus = eventBus;
            _botMoveCreator = botMoveCreator;
            _charactersMover = charactersMover;
            _creator = creator;
            _charactersStorage = charactersStorage;
            _pauseService = pauseService;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new EventTransition<PlayerMoveCompletedEvent, BotTurnState>(_eventBus),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any()),
                },
                _eventBus,
                _charactersMover,
                _creator
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new EventTransition<BotMoveCompletedEvent, PlayerTurnState>(_eventBus),
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any())
                },
                _botMoveCreator,
                _charactersMover,
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