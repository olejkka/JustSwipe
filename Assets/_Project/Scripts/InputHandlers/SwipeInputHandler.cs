using System;
using _Project.Scripts.Characters;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace _Project.Scripts.InputHandlers
{
    public class SwipeInputHandler : MonoBehaviour
    {
        private Vector2 start;
        private bool swiping;
        private float minDistance = 50f;
        
        public event Action<Vector2Int> OnPressed;
        

        private void Update()
        {
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
                        
                        OnPressed?.Invoke(dir);
                    }
                    
                    swiping = false;
                }
            }
        }
        
        private void OnEnable() => EnhancedTouchSupport.Enable();
        
        private void OnDisable() => EnhancedTouchSupport.Disable();
    }
}