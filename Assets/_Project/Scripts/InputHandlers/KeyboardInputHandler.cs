using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace _Project.Scripts.InputHandlers
{
    public class KeyboardInputHandler : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PauseService _pauseService;

        private void Update()
        {
            if (_pauseService.IsPaused)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            Vector2Int? dir = null;

            if (keyboard.wKey.wasPressedThisFrame)
                dir = Vector2Int.up;
            else if (keyboard.dKey.wasPressedThisFrame)
                dir = Vector2Int.right;
            else if (keyboard.sKey.wasPressedThisFrame)
                dir = Vector2Int.down;
            else if (keyboard.aKey.wasPressedThisFrame)
                dir = Vector2Int.left;

            if (dir.HasValue)
                _eventBus.Publish(new SwipeEvent(dir.Value));
        }
    }
}