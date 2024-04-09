using UnityEngine;

public interface IScrollBehavior
{
    Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing);
}
