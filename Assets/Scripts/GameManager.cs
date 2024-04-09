using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputMediatorSO _input;
    [SerializeField] private MonsterSO _monsters;

    [Header("UI")]
    [SerializeField] private List<Image> _monsterFooters = new List<Image>();
    [SerializeField] private InfinityScroll _infiniteScroll;
    [SerializeField] private Sprite _defaultSprite;
    private List<Sprite> _monsterChoices = new List<Sprite>();
    private bool _isScrolling;

    private void OnEnable()
    {
        _input.OnSpaceClicked += OnSpaceClicked;
    }

    private void OnDisable()
    {
        _input.OnSpaceClicked -= OnSpaceClicked;
    }

    private void OnSpaceClicked()
    {
        _isScrolling = true;
        ClearMonsterChoices();
        _infiniteScroll.Init(_monsters.GetAssets);
    }

    private void ClearMonsterChoices()
    {
        _monsterChoices.Clear();
        _monsterFooters.ForEach(v => v.sprite = _defaultSprite);
    }

    public void OnMonsterClicked(Image monster)
    {
        if (!_isScrolling && _monsterChoices.Count < 3) return;

        _monsterChoices.Add(monster.sprite);
        _monsterFooters[_monsterChoices.Count - 1].sprite = _monsterChoices[_monsterChoices.Count - 1];

        _infiniteScroll.StopScroll(_monsterChoices);
    }
}
