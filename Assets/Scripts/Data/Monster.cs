using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Monster : ScriptableObject
{
    public int Type => m_Type;
    public int Tier => m_Tier;
    public Faction Faction => m_Faction;
    public MapObjectVisualData MapVisualData => m_MapVisualData;

    [SerializeField] int m_Type;
    [SerializeField] int m_Tier;
    [SerializeField] Faction m_Faction;
    [SerializeField] MapObjectVisualData m_MapVisualData;
}
