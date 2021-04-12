using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes 3/Starting Bonus List")]
public class StartingBonusList : ScriptableObject
{
    public List<StartingBonus> StartingBonuses => m_StartingBonuses;

    [SerializeField] List<StartingBonus> m_StartingBonuses;
}
