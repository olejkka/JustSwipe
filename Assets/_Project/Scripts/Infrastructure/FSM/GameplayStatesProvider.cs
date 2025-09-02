using System;
using System.Collections.Generic;
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
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersMovementOrchestrator _charactersMovementOrchestrator;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharacterStatsConfig _characterStatsConfig;
        private readonly PauseService _pauseService;
        private readonly PlayerInputHandler _playerInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly TurnService _turnService;


        public GameplayStatesProvider(
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            CharactersMovementOrchestrator charactersMovementOrchestrator,
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
            _charactersMovementOrchestrator = charactersMovementOrchestrator;
            _playerInputHandler = playerInputHandler;
            _turnService = turnService;
            _pauseService = pauseService;
            _charactersStorage = charactersStorage;
            _characterCreator = characterCreator;
            _characterStatsConfig = characterStatsConfig;
        }

        public IReadOnlyList<IState> GetStates()
        {
            return new IState[]
            {

            };
        }

        public Type GetStartState() => typeof(PlayerTurnState);
    }
}