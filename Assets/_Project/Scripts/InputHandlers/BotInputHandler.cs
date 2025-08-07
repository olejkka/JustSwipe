using System;
using UnityEngine;
using _Project.Scripts.Characters;
using Random = UnityEngine.Random;

namespace _Project.Scripts.InputHandlers
{
    public class BotInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2Int, Team> OnPressed;
        float timer;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer < 1f) return;
            timer = 0f;
            var dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            var dir = dirs[Random.Range(0, dirs.Length)];
            OnPressed?.Invoke(dir, Team.Bot);
        }
    }
}