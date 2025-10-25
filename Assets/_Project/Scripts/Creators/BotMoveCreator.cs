using UnityEngine;

namespace _Project.Scripts.Creators
{
    public class BotMoveCreator
    {
        private readonly Vector2Int[] _directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
  
        
        public Vector2Int GenerateRandomDirection() => 
            _directions[Random.Range(0, _directions.Length)];
    }
}