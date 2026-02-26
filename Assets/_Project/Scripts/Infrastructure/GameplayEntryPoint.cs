using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM;
using _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayEntryPoint : IStartable
    {
        private readonly GameplayStateMachine _stateMachine;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsCreator;
        private readonly GameplayMoney _gameplayMoney;
        

        public GameplayEntryPoint(
            GameplayStateMachine stateMachine,
            InitialGameplayConfig initialGameplayConfig,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            GameplayMoney gameplayMoney
        )
        {
            _stateMachine = stateMachine;
            _initialGameplayConfig = initialGameplayConfig;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _gameplayMoney = gameplayMoney;
        }

        public void Start()
        {
            _positionsCreator.Create();
            _gameplayMoney.SetAmount(_initialGameplayConfig.MoneyCount);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.PlayerCharacter);
            _characterCreator.CreateOnRandomPos(_initialGameplayConfig.BotCharacter);

            _stateMachine.EnterState<PlayerTurnState>();
        }
    }
}