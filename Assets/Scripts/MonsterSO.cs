using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObjects/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] private List<Sprite> Monsters;

    public int GetIndex(Sprite monster) => Monsters.IndexOf(monster);
}
