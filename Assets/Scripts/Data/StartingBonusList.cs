using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StartingBonusList : ScriptableObject
{
    public List<StartingBonus> StartingBonuses => m_StartingBonuses;

    [SerializeField] List<StartingBonus> m_StartingBonuses;
}
