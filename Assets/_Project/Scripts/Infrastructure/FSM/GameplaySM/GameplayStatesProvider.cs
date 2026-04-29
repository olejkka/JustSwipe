using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;
        private readonly BotSpawnChancesConfig _botSpawnChancesConfig;


        public GameplayStatesProvider(
            EventBus.EventBus eventBus,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage,
            BotSpawnChancesConfig botSpawnChancesConfig
        )
        {
            _eventBus = eventBus;
            _botMoveCreator = botMoveCreator;
            _charactersMover = charactersMover;
            _characterCreator = characterCreator;
            _charactersStorage = charactersStorage;
            _botSpawnChancesConfig = botSpawnChancesConfig;
        }
        
        public IReadOnlyList<IState> GetStates()
        {
            var playerTurnState = new PlayerTurnState(
                new ITransition[]
                {
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any()),
                    new EventTransition<PlayerMoveCompletedEvent, BotTurnState>(_eventBus),
                },
                _eventBus,
                _charactersMover
            );
            
            var botTurnState = new BotTurnState(
                new ITransition[]
                {
                    new TransitionTo<EndGameState>(() => !_charactersStorage.GetCharactersByTeam(Team.Player).Any()),
                    new EventTransition<BotMoveCompletedEvent, PlayerTurnState>(_eventBus),
                },
                _botSpawnChancesConfig,
                _botMoveCreator,
                _charactersMover,
                _characterCreator,
                _charactersStorage
            );
            
            var endGameState = new EndGameState(
                new ITransition[]
                {
                },
                _eventBus
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