using UnityEngine;

namespace _Project.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdapter : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        
        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;
            
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }
    }
}