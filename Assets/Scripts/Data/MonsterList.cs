using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Monster List")]
public class MonsterList : ScriptableObject
{
    public List<Monster> Monsters => m_Monsters;

    [SerializeField] List<Monster> m_Monsters;
}
