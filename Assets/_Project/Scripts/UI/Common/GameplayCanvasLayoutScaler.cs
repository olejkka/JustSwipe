using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Common
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public class GameplayCanvasLayoutScaler : MonoBehaviour
    {
        [SerializeField] private Vector2 _referenceResolution = new(1080f, 1920f);
        [SerializeField] private bool _scaleRectTransforms = true;
        [SerializeField] private bool _scaleLayoutGroups = true;
        [SerializeField] private bool _scaleLayoutElements = true;
        [SerializeField] private bool _scaleFonts = true;

        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            var root = transform as RectTransform;
            if (root == null)
                return;

            float factor = CalculateFactor(root, canvas);
            if (Mathf.Approximately(factor, 1f))
                return;

            var rects = GetComponentsInChildren<RectTransform>(true);
            for (var i = 0; i < rects.Length; i++)
                ScaleObject(rects[i], factor);
        }

        private float CalculateFactor(RectTransform root, Canvas canvas)
        {
            // Canvas units after CanvasScaler (not raw Screen pixels).
            var size = root.rect.size;
            if (size.x <= 0f || size.y <= 0f)
                size = new Vector2(Screen.width, Screen.height) / Mathf.Max(canvas.scaleFactor, 0.0001f);

            float widthFactor = size.x / _referenceResolution.x;
            float heightFactor = size.y / _referenceResolution.y;
            return Mathf.Min(widthFactor, heightFactor);
        }

        private void ScaleObject(RectTransform rect, float factor)
        {
            if (_scaleRectTransforms)
                ScaleRectTransform(rect, factor);

            if (_scaleLayoutGroups)
            {
                if (rect.TryGetComponent<GridLayoutGroup>(out var grid))
                    ScaleGrid(grid, factor);
                else if (rect.TryGetComponent<HorizontalOrVerticalLayoutGroup>(out var layout))
                    ScaleHorizontalOrVertical(layout, factor);
            }

            if (_scaleLayoutElements &&
                rect.TryGetComponent<LayoutElement>(out var layoutElement))
                ScaleLayoutElement(layoutElement, factor);

            if (_scaleFonts)
            {
                if (rect.TryGetComponent<TMP_Text>(out var tmp))
                    ScaleTmp(tmp, factor);
                else if (rect.TryGetComponent<Text>(out var text))
                    text.fontSize = Mathf.Max(1, Mathf.RoundToInt(text.fontSize * factor));
            }
        }

        private static void ScaleRectTransform(RectTransform rect, float factor)
        {
            // Stretch anchors: offsets are distances from edges — scale them.
            bool stretchX = !Mathf.Approximately(rect.anchorMin.x, rect.anchorMax.x);
            bool stretchY = !Mathf.Approximately(rect.anchorMin.y, rect.anchorMax.y);

            if (stretchX || stretchY)
            {
                var offsetMin = rect.offsetMin;
                var offsetMax = rect.offsetMax;
                if (stretchX)
                {
                    offsetMin.x *= factor;
                    offsetMax.x *= factor;
                }
                if (stretchY)
                {
                    offsetMin.y *= factor;
                    offsetMax.y *= factor;
                }
                rect.offsetMin = offsetMin;
                rect.offsetMax = offsetMax;
            }
            else
            {
                rect.anchoredPosition *= factor;
                rect.sizeDelta *= factor;
            }
        }

        private static void ScaleGrid(GridLayoutGroup grid, float factor)
        {
            grid.padding = ScalePadding(grid.padding, factor);
            grid.spacing *= factor;
            grid.cellSize *= factor;
        }

        private static void ScaleHorizontalOrVertical(
            HorizontalOrVerticalLayoutGroup layout,
            float factor)
        {
            layout.padding = ScalePadding(layout.padding, factor);
            layout.spacing *= factor;
        }

        private static RectOffset ScalePadding(RectOffset padding, float factor)
        {
            return new RectOffset(
                Mathf.RoundToInt(padding.left * factor),
                Mathf.RoundToInt(padding.right * factor),
                Mathf.RoundToInt(padding.top * factor),
                Mathf.RoundToInt(padding.bottom * factor));
        }

        private static void ScaleLayoutElement(LayoutElement element, float factor)
        {
            if (element.minWidth >= 0f)
                element.minWidth *= factor;
            if (element.minHeight >= 0f)
                element.minHeight *= factor;
            if (element.preferredWidth >= 0f)
                element.preferredWidth *= factor;
            if (element.preferredHeight >= 0f)
                element.preferredHeight *= factor;
            // flexibleWidth/Height — веса, не масштабируем.
        }

        private static void ScaleTmp(TMP_Text tmp, float factor)
        {
            tmp.fontSize *= factor;
            if (tmp.enableAutoSizing)
            {
                tmp.fontSizeMin *= factor;
                tmp.fontSizeMax *= factor;
            }
        }
    }
}