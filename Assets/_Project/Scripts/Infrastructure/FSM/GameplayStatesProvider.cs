using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.FSM;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.Infrastructure.GameplayPhases;
using _Project.Scripts.InputHandlers;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class GameplayStatesProvider : IGameplayStatesProvider
    {
        private readonly PauseService _pauseService;
        private readonly PhaseHandler _phaseHandler;


        public GameplayStatesProvider(
            PauseService pauseService,
            PhaseHandler phaseHandler
        )
        {
            _pauseService = pauseService;
            _phaseHandler = phaseHandler;
        }

        public IReadOnlyList<IState> GetStates()
        {
            var gameplayState = new GameplayState(
                new ITransition[]
                {
                    new TransitionTo<PauseState>(() => _pauseService.IsPaused)
                },
                _phaseHandler,
                _pauseService
            );
            

            var pauseState = new PauseState(
                new ITransition[]
                {
                    new TransitionTo<GameplayState>(() => !_pauseService.IsPaused)
                }
            );

            var endGameState = new EndGameState(
                new ITransition[]
                {
                }
            );

            return new IState[]
            {
                gameplayState,
                pauseState,
                endGameState
            };
        }

        public Type GetStartState()
        {
            return typeof(GameplayState);
        }
    }
}