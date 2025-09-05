using _Project.Scripts.Characters;
using _Project.Scripts.InputHandlers;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases
{
    public class InputReadingPhase : Phase
    {
        private readonly BotInputHandler _botInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;

        private readonly InputStorage _inputStorage;


        public InputReadingPhase(
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            InputStorage inputStorage
        )
        {
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _inputStorage = inputStorage;
        }

        public override void Enter()
        {
            if (_humanPhase)
                _swipeInputHandler.OnPressed += ReadInput;
            else
                _botInputHandler.OnPressed += ReadInput;
        }

        private void ReadInput(Vector2Int vector, Team team)
        {
            _inputStorage.InputVector = vector;
            _swipeInputHandler.OnPressed -= ReadInput;
            _botInputHandler.OnPressed -= ReadInput;
            _humanPhase = !_humanPhase;
            Exit();
        }
    }
}