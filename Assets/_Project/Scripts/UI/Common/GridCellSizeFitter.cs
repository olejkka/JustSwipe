using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Common
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridCellSizeFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private float _maxCellSize = 250f;
        [SerializeField] private float _minCellSize = 200f;

        private void OnEnable()
        {
            UpdateCellSize();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateCellSize();
        }

        private void UpdateCellSize()
        {
            if (_rectTransform == null || _grid == null)
                return;

            int columns = _grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount
                ? Mathf.Max(1, _grid.constraintCount)
                : 2;

            float spacing = _grid.spacing.x * (columns - 1);
            float padding = _grid.padding.left + _grid.padding.right;
            float availableWidth = _rectTransform.rect.width - padding - spacing;

            if (availableWidth <= 0f)
                return;

            float cell = Mathf.Clamp(availableWidth / columns, _minCellSize, _maxCellSize);
            var newSize = new Vector2(cell, cell);

            if (_grid.cellSize == newSize)
                return;

            _grid.cellSize = newSize;
        }
    }
}