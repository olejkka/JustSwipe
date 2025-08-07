using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Characters
{
    public class CharacterView : MonoBehaviour
    {
        private Character _data;
        private Tilemap _tilemap;

        private void OnDestroy()
        {
            if (_data != null)
                _data.OnPositionChanged -= UpdatePosition;
        }

        public void Init(Character data, Tilemap tilemap)
        {
            _data = data;
            _tilemap = tilemap;
            _data.OnPositionChanged += UpdatePosition;
            UpdatePosition(_data.Position);

            Debug.Log(
                $"[CharacterViewInstantiator] Init - {_data.Position}, {_data.Team}, {_data.Health}, {_data.Damage}");
        }

        private void UpdatePosition(Vector2Int pos)
        {
            var cell = new Vector3Int(pos.x, pos.y, 0);
            var worldPos = _tilemap.CellToWorld(cell);
            transform.position = worldPos;

            Debug.Log($"[CharacterViewInstantiator] Update Position - {worldPos}");
        }
    }
}