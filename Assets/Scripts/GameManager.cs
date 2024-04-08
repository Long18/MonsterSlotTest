using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputMediatorSO _input;
    [SerializeField] private MonsterSO _monsters;
    [SerializeField] private List<Image> monsterFooters = new List<Image>();
    private List<Image> monsterChoices = new List<Image>();

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
        Debug.Log("Space clicked");
    }

    public void OnMonsterClicked(Image monster)
    {
        if (monsterChoices.Count >= 3) return;

        monsterChoices.Add(monster);
        monsterFooters[monsterChoices.Count - 1].sprite = monsterChoices[monsterChoices.Count - 1].sprite;


        if (monsterChoices.Count == 3)
        {
            Debug.Log(_monsters.GetIndex(monsterChoices[0].sprite));
            Debug.Log(_monsters.GetIndex(monsterChoices[1].sprite));
            Debug.Log(_monsters.GetIndex(monsterChoices[2].sprite));
            Debug.Log("Full!!");
        }
    }
}
