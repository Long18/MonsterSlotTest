using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _stopPosition;
    [SerializeField] private UISlot _monsterPrefab;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _rangeOfVelocity;
    [SerializeField] private float _itemSpacing;

    private bool _isStopScroll;
    private bool _isMovingUp;
    private Transform _targetSlowChildTransform;
    private Vector3 _targetOriginPosition;
    private Vector3 _currentScrollPosition => _scrollRect.content.position;
    private Vector2 _scrollRectVelocity => new Vector2(0, _scrollSpeed);
    private Dictionary<string, GameObject> _monsterUIs = new Dictionary<string, GameObject>();

    private void Start() => _scrollRect.onValueChanged.AddListener(OnScroll);

    public void Init(List<Sprite> monsters)
    {
        Reset();
        Clear();
        Create(monsters);
    }
    public void StopScroll(List<Sprite> monsterChoices)
    {
        _isStopScroll = true;

        int stopIndex = Mathf.Clamp(1, 0, monsterChoices.Count - 1);

        for (int i = 0; i < monsterChoices.Count; i++)
        {
            Transform lastMonster = _scrollRect.content.GetChild(_scrollRect.content.childCount - 1);
            Transform showMonsterPos = CreateMonster(lastMonster.position, monsterChoices[i]);

            if (i == stopIndex)
            {
                _targetSlowChildTransform = showMonsterPos;
                _targetOriginPosition = _targetSlowChildTransform.position;
            }
        }
    }

    private void OnScroll(Vector2 value)
    {
        _isMovingUp = _scrollRect.velocity.y > 0;

        int monsterIndex = _isMovingUp ? _scrollRect.content.childCount - 1 : 0;
        RectTransform currentMonster = _scrollRect.content.GetChild(monsterIndex).GetComponent<RectTransform>();

        if (!IsReachThreshold(currentMonster)) return;

        var lastMonsterIndex = _isMovingUp ? 0 : _scrollRect.content.childCount - 1;
        var lastMonster = _scrollRect.content.GetChild(lastMonsterIndex);

        var lastPosition = lastMonster.position;

        if (_isMovingUp) lastPosition.y = lastMonster.position.y - currentMonster.rect.height - _itemSpacing;
        else lastPosition.y = lastMonster.position.y + currentMonster.rect.height + _itemSpacing;

        currentMonster.position = lastPosition;
        currentMonster.SetSiblingIndex(lastMonsterIndex);
    }

    private bool IsReachThreshold(RectTransform currentMonster)
    {
        if (_isMovingUp) return currentMonster.position.y - currentMonster.rect.height > transform.position.y + _rangeOfVelocity;
        else return currentMonster.position.y + currentMonster.rect.height < transform.position.y - _rangeOfVelocity;
    }

    public void Update()
    {
        if (_scrollRect.content.childCount <= 0) return;
        _scrollRect.velocity = _isStopScroll ? CalculateVelocity() : _scrollRectVelocity;
    }

    private Vector2 CalculateVelocity()
    {
        float targetOriginDistance = _targetOriginPosition.y - _stopPosition.position.y;
        float targetCurrentDistance = _targetSlowChildTransform.position.y - _stopPosition.position.y;
        float velocityIntensity = Mathf.Abs(targetCurrentDistance) / targetOriginDistance;

        if (targetCurrentDistance > 0)
        {
            return Vector2.Lerp(Vector2.zero, _scrollRectVelocity, velocityIntensity);
        }
        return Vector2.zero;
    }

    private void Clear()
    {
        foreach (var monsterUI in _monsterUIs) Destroy(monsterUI.Value);
        _monsterUIs.Clear();
    }

    private void Reset() => _isStopScroll = false;

    private void Create(List<Sprite> monsters) => monsters.ForEach(monster => CreateMonster(_currentScrollPosition, monster));

    private Transform CreateMonster(Vector3 position, Sprite sprite)
    {
        UISlot slot = Instantiate(_monsterPrefab, _scrollRect.content);
        slot.Init(sprite);

        RectTransform rect = slot.GetComponent<RectTransform>();
        position.y += rect.rect.height + _itemSpacing;

        rect.position = position;
        rect.SetAsLastSibling();

        _monsterUIs.TryAdd(sprite.name, slot.gameObject);

        return rect;
    }
}
