using UnityEngine;
using UnityEngine.UI;

public class ThresholdChecker
{
    private readonly ScrollRect _scrollRect;
    private readonly float _rangeOfVelocity;
    private readonly Transform _transform;

    public ThresholdChecker(ScrollRect scrollRect, float rangeOfVelocity, Transform transform)
    {
        _scrollRect = scrollRect;
        _rangeOfVelocity = rangeOfVelocity;
        _transform = transform;
    }

    public bool IsReachThreshold(RectTransform currentMonster)
    {
        if (_scrollRect.velocity.y > 0) return currentMonster.position.y - currentMonster.rect.height > _transform.position.y + _rangeOfVelocity;
        else return currentMonster.position.y + currentMonster.rect.height < _transform.position.y - _rangeOfVelocity;
    }
}

