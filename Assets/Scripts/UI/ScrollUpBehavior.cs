using UnityEngine;

public class ScrollUpBehavior : IScrollBehavior
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing)
    {
        Vector3 lastPosition = lastMonster.position;
        lastPosition.y = lastMonster.position.y - currentMonster.rect.height - itemSpacing;
        return lastPosition;
    }
}

public class ScrollDownBehavior : IScrollBehavior
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing)
    {
        Vector3 lastPosition = lastMonster.position;
        lastPosition.y = lastMonster.position.y + currentMonster.rect.height + itemSpacing;
        return lastPosition;
    }
}

public class ScrollBehavior : IScrollBehavior
{
    public Vector2 Scroll(RectTransform currentMonster, Transform lastMonster, float itemSpacing) => Vector2.zero;
}