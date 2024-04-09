using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinityScroll : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _stopPosition;
    [SerializeField] private UISlot _monsterPrefab;

    [Header("Config")]
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _rangeOfVelocity;
    [SerializeField] private float _itemSpacing;

    private bool _isStopScroll;
    private IScrollable _scrollUp;
    private IScrollable _scrollDown;
    private ThresholdChecker _thresholdChecker;
    private Transform _slowChildTransform;
    private Vector3 _originPosition;
    private Vector2 _scrollRectVelocity => new Vector2(0, _scrollSpeed);
    private List<UISlot> _monsterUIs = new List<UISlot>();

    private void Start()
    {
        _scrollUp = new ScrollUp();
        _scrollDown = new ScrollDown();
        _thresholdChecker = new ThresholdChecker(_scrollRect, _rangeOfVelocity, _stopPosition);
    }

    public void Init(List<Sprite> monsters)
    {
        Reset();
        Clear();
        Create(monsters);
    }

    public void StopScroll(List<Sprite> monsterChoices)
    {
        _isStopScroll = true;
        bool isMovingUp = _scrollRect.velocity.y > 0;

        for (int i = 0; i < monsterChoices.Count; i++)
        {
            Transform lastMonster = _scrollRect.content.GetChild(GetLastMonsterIndex(isMovingUp));
            Transform monster = CreateMonster(lastMonster.position, monsterChoices[i]);
            SetSlowChildTranform(monster, monsterChoices, i);
        }
    }

    private void SetSlowChildTranform(Transform monsterTransform, List<Sprite> monsterChoices, int index)
    {
        int stopIndex = Mathf.Clamp(1, 0, monsterChoices.Count - 1);
        if (index != stopIndex) return;

        _slowChildTransform = monsterTransform;
        _originPosition = _slowChildTransform.position;
    }

    public void OnScroll(Vector2 value)
    {
        bool isMovingUp = _scrollRect.velocity.y > 0;

        Transform monster = _scrollRect.content.GetChild(GetMonsterIndex(isMovingUp));
        RectTransform currentMonster = monster.GetComponent<RectTransform>();

        if (!_thresholdChecker.IsReachThreshold(currentMonster)) return;

        Transform lastMonster = _scrollRect.content.GetChild(GetLastMonsterIndex(isMovingUp));

        currentMonster.position = isMovingUp ?
            _scrollUp.Scroll(currentMonster, lastMonster, _itemSpacing) :
            _scrollDown.Scroll(currentMonster, lastMonster, _itemSpacing);

        currentMonster.SetSiblingIndex(GetLastMonsterIndex(isMovingUp));
    }

    private void Update()
    {
        if (_scrollRect.content.childCount <= 0) return;
        _scrollRect.velocity = _isStopScroll ? CalculateVelocity() : _scrollRectVelocity;
    }

    private Vector2 CalculateVelocity()
    {
        float originDistance = _originPosition.y - _stopPosition.position.y;
        float currentDistance = _slowChildTransform.position.y - _stopPosition.position.y;
        float velocityIntensity = Mathf.Abs(currentDistance) / originDistance;

        if (currentDistance > 0) return Vector2.Lerp(Vector2.zero, _scrollRectVelocity, velocityIntensity);
        return Vector2.zero;
    }
    private int GetLastMonsterIndex(bool isMovingUp) => isMovingUp ? 0 : _scrollRect.content.childCount - 1;
    private int GetMonsterIndex(bool isMovingUp) => isMovingUp ? _scrollRect.content.childCount - 1 : 0;

    private void Clear()
    {
        foreach (var monsterUI in _monsterUIs) Destroy(monsterUI.gameObject);
        _monsterUIs.Clear();
    }

    private void Reset() => _isStopScroll = false;

    private void Create(List<Sprite> monsters)
    {
        Vector3 nextMonsterPosition = _scrollRect.content.position;
        monsters.ForEach(monster =>
        {
            Transform monsterTransform = CreateMonster(nextMonsterPosition, monster);
            nextMonsterPosition = monsterTransform.position;
        });
    }

    private Transform CreateMonster(Vector3 position, Sprite sprite)
    {
        UISlot slot = Instantiate(_monsterPrefab, _scrollRect.content);
        slot.Init(sprite);

        RectTransform rect = slot.GetComponent<RectTransform>();
        position.y += rect.rect.height + _itemSpacing;

        rect.position = position;
        rect.SetAsLastSibling();

        _monsterUIs.Add(slot);

        return rect;
    }

}
