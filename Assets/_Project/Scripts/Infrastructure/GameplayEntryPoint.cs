using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.Infrastructure.FSM.States.GameplayStates;
using _Project.Scripts.Wallet;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class GameplayEntryPoint : IStartable
    {
        private readonly GameplayStateMachine _stateMachine;
        private readonly CharacterCreator _characterCreator;
        private readonly PositionsCreator _positionsCreator;
        private readonly Money _money;

        public GameplayEntryPoint(
            GameplayStateMachine stateMachine,
            PositionsCreator positionsCreator,
            CharacterCreator characterCreator,
            Money money
        )
        {
            _stateMachine = stateMachine;
            _positionsCreator = positionsCreator;
            _characterCreator = characterCreator;
            _money = money;
        }

        public void Start()
        {
            _positionsCreator.Create();
            _money.SetAmount(10);
            _characterCreator.CreateOnRandomPos(CharacterType.Main);
            _characterCreator.CreateOnRandomPos(CharacterType.Bot_1);

            _stateMachine.EnterState<PlayerTurnState>();
        }
    }
}