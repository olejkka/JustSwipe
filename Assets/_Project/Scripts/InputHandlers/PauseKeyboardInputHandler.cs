using System;
using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.InputHandlers
{
    public class PauseKeyboardInputHandler : MonoBehaviour
    {
        public event Action OnPauseButtonPressed;

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape");
                OnPauseButtonPressed?.Invoke();
            }
        }
    }
}