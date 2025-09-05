using _Project.Scripts.Characters;
using _Project.Scripts.Infrastructure.GameplayPhases.Phases;
using _Project.Scripts.InputHandlers;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases
{
    public class InputReadingPhase : Phase
    {
        private readonly SwipeInputHandler _swipeInputHandler;

        private readonly InputStorage _inputStorage;


        public InputReadingPhase(
            SwipeInputHandler swipeInputHandler,
            InputStorage inputStorage
        )
        {
            _swipeInputHandler = swipeInputHandler;
            _inputStorage = inputStorage;
        }

        public override void Enter()
        {
            _swipeInputHandler.OnPressed += ReadInput;
        }

        private void ReadInput(Vector2Int vector)
        {
            _inputStorage.InputVector = vector;
            _swipeInputHandler.OnPressed -= ReadInput;
            
            Exit();
        }
    }
}