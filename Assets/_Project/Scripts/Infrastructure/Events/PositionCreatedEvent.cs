using UnityEngine;

namespace _Project.Scripts.Infrastructure.Events
{
    public class PositionCreatedEvent
    {
        public Vector2Int Position { get; }
        public PositionCreatedEvent(Vector2Int position) => Position = position;
    }
}