using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_GoblinG;
    [SerializeField] Building m_Goblin2G;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_GoblinG.gameObject.SetActive(false);
                m_Goblin2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_GoblinG.gameObject.SetActive(true);
                m_Goblin2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GoblinG.gameObject.SetActive(false);
            m_Goblin2G.gameObject.SetActive(false);
        }
    }
}
