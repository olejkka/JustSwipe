using System;
using UnityEngine;

namespace _Project.Scripts.InputHandlers
{
    public class KeyboardInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2Int> OnPressed;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) 
                OnPressed?.Invoke(Vector2Int.up);
            if (Input.GetKeyDown(KeyCode.S)) 
                OnPressed?.Invoke(Vector2Int.down);
            if (Input.GetKeyDown(KeyCode.A)) 
                OnPressed?.Invoke(Vector2Int.left);
            if (Input.GetKeyDown(KeyCode.D)) 
                OnPressed?.Invoke(Vector2Int.right);
        }
    }
}