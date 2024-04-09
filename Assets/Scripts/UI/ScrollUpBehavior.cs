using UnityEngine;

public class ScrollUp : IScrollable
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing)
    {
        Vector3 lastPosition = lastMonster.position;
        lastPosition.y = lastMonster.position.y - currentMonster.rect.height - itemSpacing;
        return lastPosition;
    }
}

public class ScrollDown : IScrollable
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing)
    {
        Vector3 lastPosition = lastMonster.position;
        lastPosition.y = lastMonster.position.y + currentMonster.rect.height + itemSpacing;
        return lastPosition;
    }
}

public class ScrollBase : IScrollable
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing) => Vector2.zero;
}