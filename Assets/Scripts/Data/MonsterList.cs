using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterList : ScriptableObject
{
    public List<Monster> Monsters => m_Monsters;

    [SerializeField] List<Monster> m_Monsters;
}
