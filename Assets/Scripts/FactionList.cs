using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FactionList : ScriptableObject
{
    public List<Faction> Factions => m_Factions;

    [SerializeField] List<Faction> m_Factions;
}
