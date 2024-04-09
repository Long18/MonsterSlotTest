using UnityEngine;

public interface IScrollable
{
    Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing);
}
