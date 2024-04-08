using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSO monsters;

    [SerializeField] private List<Image> monsterFooters = new List<Image>();
    private List<Image> monsterChoices = new List<Image>();

    public void OnMonsterClicked(Image monster)
    {
        if (monsterChoices.Count >= 3) return;

        monsterChoices.Add(monster);
        monsterFooters[monsterChoices.Count - 1].sprite = monsterChoices[monsterChoices.Count - 1].sprite;


        if (monsterChoices.Count == 3)
        {
            Debug.Log(monsters.GetIndex(monsterChoices[0].sprite));
            Debug.Log(monsters.GetIndex(monsterChoices[1].sprite));
            Debug.Log(monsters.GetIndex(monsterChoices[2].sprite));
            Debug.Log("Full!!");
        }
    }
}
