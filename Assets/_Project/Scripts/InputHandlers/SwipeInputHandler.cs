using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace _Project.Scripts.InputHandlers
{
    public class SwipeInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action<Vector2Int> OnPressed;
        Vector2 start;
        bool swiping;
        float minDistance = 50f;

        void OnEnable()  => EnhancedTouchSupport.Enable();
        void OnDisable() => EnhancedTouchSupport.Disable();

        void Update()
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
                        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                            OnPressed?.Invoke(delta.x > 0 ? Vector2Int.right : Vector2Int.left);
                        else
                            OnPressed?.Invoke(delta.y > 0 ? Vector2Int.up    : Vector2Int.down);
                    }
                    swiping = false;
                }
            }
        }
    }

}