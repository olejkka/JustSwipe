using UnityEngine;

namespace _Project.Scripts.Characters.Projectiles
{
    public class Projectile
    {
        public string DefinitionId { get; }
        public Character Shooter { get; }
        public Vector2Int Direction { get; }
        public Vector2Int StartPosition { get; }
        public Vector2Int EndPosition { get; }
        public Character Target { get; }


        public Projectile(
            string definitionId,
            Character shooter,
            Vector2Int direction,
            Vector2Int startPosition,
            Vector2Int endPosition,
            Character target)
        {
            DefinitionId = definitionId;
            Shooter = shooter;
            Direction = direction;
            StartPosition = startPosition;
            EndPosition = endPosition;
            Target = target;
        }
    }
}
