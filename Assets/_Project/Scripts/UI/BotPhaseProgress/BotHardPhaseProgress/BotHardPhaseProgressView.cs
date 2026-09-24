using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.BotPhaseProgress.BotHardPhaseProgress
{
    public class BotHardPhaseProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _icon;
        [SerializeField] private Color _activeColor;

        private const float BlinkHalfDuration = 0.6f;

        private Color _inactiveColor;
        private Tween _blinkTween;


        private void Awake()
        {
            _inactiveColor = _icon.color;
        }

        private void OnDestroy()
        {
            _blinkTween?.Kill();
        }

        public void SetProgress(int killed, int threshold)
        {
            _progress.text = $"{killed}/{threshold}";
            _fillImage.fillAmount = threshold > 0 ? (float)killed / threshold : 0f;
        }

        public void SetPhaseActive(bool active)
        {
            _progress.enabled = !active;
            _fillImage.enabled = !active;

            if (!active)
            {
                _blinkTween?.Kill();
                _blinkTween = null;
                _icon.color = _inactiveColor;
                return;
            }

            if (_blinkTween != null && _blinkTween.IsActive())
                return;

            _icon.color = _inactiveColor;
            _blinkTween = _icon
                .DOColor(_activeColor, BlinkHalfDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}