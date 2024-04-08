using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _rangeOfVelocity;

    [SerializeField] private float _itemSpacing;
    [SerializeField] private Transform _stopPosition;
    [SerializeField] private UISlot _monsterPrefab;

    private Dictionary<string, GameObject> _monsterUIs = new Dictionary<string, GameObject>();

    private bool _isDecreaseVelocity;

    private Transform _targetSlowChildTransform;
    private Vector3 _targetOriginPosition;
    private bool _isDragPosition;

    private Vector3 _currentScrollPosition => _scrollRect.content.position;
    private Vector2 _scrollRectVelocity => new Vector2(0, _scrollSpeed);

    private void Start() => _scrollRect.onValueChanged.AddListener(OnScroll);

    public void Init(List<Sprite> monsters)
    {
        Clear();
        Create(monsters);
    }

    private void OnScroll(Vector2 value)
    {
        _isDragPosition = _scrollRect.velocity.y > 0;

        int monsterIndex = _isDragPosition ? _scrollRect.content.childCount - 1 : 0;
        RectTransform currentMonster = _scrollRect.content.GetChild(monsterIndex).GetComponent<RectTransform>();

        if (!IsReachThreshold(currentMonster)) return;

        var lastMonsterIndex = _isDragPosition ? 0 : _scrollRect.content.childCount - 1;
        var lastMonster = _scrollRect.content.GetChild(lastMonsterIndex);

        var lastPosition = lastMonster.position;

        if (_isDragPosition) lastPosition.y = lastMonster.position.y - currentMonster.rect.height - _itemSpacing;
        else lastPosition.y = lastMonster.position.y + currentMonster.rect.height + _itemSpacing;

        currentMonster.position = lastPosition;
        currentMonster.SetSiblingIndex(lastMonsterIndex);
    }

    private bool IsReachThreshold(RectTransform currentMonster)
    { 
        if (_isDragPosition) return currentMonster.position.y - currentMonster.rect.height > transform.position.y + _rangeOfVelocity;
        else return currentMonster.position.y + currentMonster.rect.height < transform.position.y - _rangeOfVelocity;
    }

    private void Update()
    {
        // TODO: Listening to key press
        UpdateVerticalVelocity();
    }

    private void UpdateVerticalVelocity()
    {
        if (_isDecreaseVelocity)
        {
            float targetOriginDistance = _targetOriginPosition.y - _stopPosition.position.y;

            float targetCurrentDistance = _targetSlowChildTransform.position.y - _stopPosition.position.y;
            float velocityIntensity = Mathf.Abs(targetCurrentDistance) / targetOriginDistance;

            Vector2 scrollViewVelocity = new();
            if (targetCurrentDistance > 0)
            {
                scrollViewVelocity = Vector2.Lerp(Vector2.zero, _scrollRectVelocity, velocityIntensity);
            }

            _scrollRect.velocity = scrollViewVelocity;
        }
        else
        {
            _scrollRect.velocity = _scrollRectVelocity;
        }
    }

    private void Clear()
    {
        foreach (var monsterUI in _monsterUIs) Destroy(monsterUI.Value);
        _monsterUIs.Clear();
    }


    private void Create(List<Sprite> monsters)
    {
        monsters.ForEach(monster => CreateItem(_currentScrollPosition, monster));
    }

    private Transform CreateItem(Vector3 scrollPosition, Sprite sprite)
    {
        UISlot slot = Instantiate(_monsterPrefab, _scrollRect.content);
        slot.Init(sprite);

        RectTransform rect = slot.GetComponent<RectTransform>();
        rect.position = new Vector2(scrollPosition.x, scrollPosition.y + rect.rect.height + _itemSpacing);
        rect.SetAsFirstSibling();

        _monsterUIs.Add(sprite.name, slot.gameObject);

        return rect;
    }

}
