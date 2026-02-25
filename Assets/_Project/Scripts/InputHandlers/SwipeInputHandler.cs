using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using VContainer;

namespace _Project.Scripts.InputHandlers
{
    public class SwipeInputHandler : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PauseService _pauseService;
        
        private Vector2 start;
        private bool swiping;
        private float minDistance = 50f;
        

        private void Update()
        {
            if (_pauseService.IsPaused)
                return;
            
            foreach (var t in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
            {
                if (t.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    start = t.screenPosition;
                    swiping = true;
                }
                else if (t.phase == UnityEngine.InputSystem.TouchPhase.Ended && swiping)
                {
                    var delta = t.screenPosition - start;
                    
                    if (delta.magnitude >= minDistance)
                    {
                        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                        Vector2Int dir;
                        
                        if (angle >= 90f)
                            dir = Vector2Int.up;
                        else if (angle >= 0f)
                            dir = Vector2Int.right;
                        else if (angle >= -90f)
                            dir = Vector2Int.down;
                        else
                            dir = Vector2Int.left;
                        
                        _eventBus.Publish(new SwipeEvent(dir));
                    }
                    
                    swiping = false;
                }
            }
        }
        
        private void OnEnable() => EnhancedTouchSupport.Enable();
        
        private void OnDisable() => EnhancedTouchSupport.Disable();
    }
}