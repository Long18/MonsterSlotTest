using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "ScriptableObjects/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] private List<Sprite> _monsters;
    public List<Sprite> Monsters => _monsters;

    public int GetIndex(Sprite monster) => _monsters.IndexOf(monster);
}
