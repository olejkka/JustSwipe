using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class IconRotateBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private Ease _ease = Ease.OutQuad;
		
        [SerializeField] private float _shakeAngle = 12f;
        [SerializeField] private float _shakeDuration = 0.25f;
        [SerializeField] private Ease _shakeEase = Ease.OutQuad;

        private Vector3 _initialLocalEulerAngles;
		
		
        private void Awake()
        {
            _initialLocalEulerAngles = _target.localEulerAngles;
        }
		
        public void Play()
        {
            _target.DOKill();
            _target.localEulerAngles = _initialLocalEulerAngles;
			
            _target
                .DOLocalRotate(
                    _target.localEulerAngles + new Vector3(0f, 0f, -360f),
                    _duration,
                    RotateMode.FastBeyond360)
                .SetEase(_ease);
        }

        public void PlayShakeRotation()
        {
            _target.DOKill();
            _target.localEulerAngles = _initialLocalEulerAngles;
			
            Sequence seq = DOTween.Sequence();
			
            float[] offsets = { -1f, 1f, -0.8f, 0.8f, -0.5f, 0.5f, -0.2f, 0.2f };
			
            float stepDuration = _shakeDuration / (offsets.Length + 1);
			
            foreach (float offset in offsets)
            {
                seq.Append(
                    _target.DOLocalRotate(
                        _initialLocalEulerAngles + new Vector3(0f, 0f, offset * _shakeAngle), stepDuration).SetEase(_shakeEase)
                );
            }

            seq.Append(
                _target.DOLocalRotate(_initialLocalEulerAngles, stepDuration).SetEase(Ease.OutQuad)
            );
        }
    }
}