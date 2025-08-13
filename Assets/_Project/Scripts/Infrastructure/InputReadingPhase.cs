using System;
using _Project.Scripts.Characters;
using _Project.Scripts.InputHandlers;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class InputReadingPhase : Phase
    {
        private readonly KeyboardInputHandler _keyboardInputHandler;
        private readonly SwipeInputHandler _swipeInputHandler;
        private readonly BotInputHandler _botInputHandler;
        
        private InputStorage _inputStorage;


        public InputReadingPhase(
            KeyboardInputHandler keyboardInputHandler,
            SwipeInputHandler swipeInputHandler,
            BotInputHandler botInputHandler,
            InputStorage inputStorage
            )
        {
            _keyboardInputHandler = keyboardInputHandler;
            _swipeInputHandler = swipeInputHandler;
            _botInputHandler = botInputHandler;
            _inputStorage = inputStorage;
        }
        
        public override void Enter()
        {
            Debug.Log("Entering InputReadingPhase");
            
            if (_humanPhase)
            {
                _keyboardInputHandler.OnPressed += ReadInput;
                _swipeInputHandler.OnPressed += ReadInput;
            }
            else
                _botInputHandler.OnPressed += ReadInput;
        }

        private void ReadInput(Vector2Int vector, Team team)
        {
            _inputStorage.InputVector = vector;
            _keyboardInputHandler.OnPressed -= ReadInput;
            _swipeInputHandler.OnPressed -= ReadInput;
            _botInputHandler.OnPressed -= ReadInput;
            _humanPhase = !_humanPhase;
            Exit();
        }
    }
}