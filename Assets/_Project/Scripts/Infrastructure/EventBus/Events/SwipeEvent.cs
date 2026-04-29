using UnityEngine;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class SwipeEvent
    {
        public Vector2Int Direction { get; }
        
        public SwipeEvent(Vector2Int direction) => Direction = direction;
    }
}