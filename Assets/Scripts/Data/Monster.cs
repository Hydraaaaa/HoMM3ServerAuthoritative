using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Monster")]
public class Monster : ScriptableObject
{
    public int Tier => m_Tier;
    public Faction Faction => m_Faction;
    public MapObjectVisualData MapVisualData => m_MapVisualData;

    [SerializeField] int m_Tier;
    [SerializeField] Faction m_Faction;
    [SerializeField] MapObjectVisualData m_MapVisualData;
}
