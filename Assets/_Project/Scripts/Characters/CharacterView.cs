// Assets/_Project/Scripts/Characters/CharacterView.cs
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Character _data;
        private Tilemap _tilemap;

        
        public void Init(Character data, Tilemap tilemap, Sprite sprite)
        {
            _data = data;
            _tilemap = tilemap;
            
            if (_spriteRenderer != null && sprite != null)
                _spriteRenderer.sprite = sprite;
            
            _data.OnPositionChanged += UpdatePosition;
            UpdatePosition(_data.Position);
        }
        
        private void UpdatePosition(Vector2Int pos)
        {
            var cell = new Vector3Int(pos.x, pos.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);
            transform.position = worldPos;
        }
        
        private void OnDestroy()
        {
            if (_data != null)
                _data.OnPositionChanged -= UpdatePosition;
        }
    }
}