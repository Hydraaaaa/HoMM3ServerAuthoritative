using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Faction List")]
public class FactionList : ScriptableObject
{
    public List<Faction> Factions => m_Factions;

    [SerializeField] List<Faction> m_Factions;
}
