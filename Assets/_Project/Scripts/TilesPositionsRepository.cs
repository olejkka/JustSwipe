using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class TilesPositionsRepository
    {
        private HashSet<Vector2Int> _positions;
        
            
        private void AddPosition(Vector2Int pos)
        {
            _positions.Add(pos);
        }

        private bool Contains(Vector2Int pos)
        {
            return _positions.Contains(pos);
        }
    }
}