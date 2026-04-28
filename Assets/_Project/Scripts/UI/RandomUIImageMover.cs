using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RandomUIImageMover : MonoBehaviour
{
    private readonly Vector2[] _offsets =
    {
        new Vector2(0f, 50f),
        new Vector2(0f, -50f),
    };

    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private int _currentTargetIndex = -1;
    private bool _isMoving;

    private const float MoveSpeed = 3f;
    private const float ReachDistance = 0.5f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
        PickNextTarget();
        StartMove();
    }

    private void Update()
    {
        if (!_isMoving)
            return;

        _rectTransform.anchoredPosition = Vector2.MoveTowards(
            _rectTransform.anchoredPosition,
            _targetPosition,
            MoveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(_rectTransform.anchoredPosition, _targetPosition) <= ReachDistance)
        {
            PickNextTarget();
        }
    }

    private void OnDestroy()
    {
        StopMove();
    }

    public void StartMove()
    {
        _isMoving = true;
    }

    public void StopMove()
    {
        _isMoving = false;
    }

    private void PickNextTarget()
    {
        int nextIndex;

        do
        {
            nextIndex = Random.Range(0, _offsets.Length);
        }
        while (_offsets.Length > 1 && nextIndex == _currentTargetIndex);

        _currentTargetIndex = nextIndex;
        _targetPosition = _startPosition + _offsets[_currentTargetIndex];
    }
}