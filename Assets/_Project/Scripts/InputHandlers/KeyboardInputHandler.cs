using System;
using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.InputHandlers
{
    public class KeyboardInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2Int, Team> OnPressed;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) 
                OnPressed?.Invoke(Vector2Int.up, Team.Player);
            if (Input.GetKeyDown(KeyCode.S)) 
                OnPressed?.Invoke(Vector2Int.down, Team.Player);
            if (Input.GetKeyDown(KeyCode.A)) 
                OnPressed?.Invoke(Vector2Int.left, Team.Player);
            if (Input.GetKeyDown(KeyCode.D)) 
                OnPressed?.Invoke(Vector2Int.right, Team.Player);
        }
    }
}