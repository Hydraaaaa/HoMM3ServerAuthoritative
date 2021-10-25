using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuildings : TownBuildings
{
    [Space]
    [SerializeField] Building m_WarrenG;
    [SerializeField] Building m_Warren2G;
    [SerializeField] Building m_ManaVortexMageGuild5;

    public override void SetBuildings(BuiltBuildings a_Data, bool a_CanBuildShipyard)
    {
        base.SetBuildings(a_Data, a_CanBuildShipyard);

        if (a_Data.Dwelling1Growth)
        {
            if (a_Data.Dwelling1Up)
            {
                m_WarrenG.gameObject.SetActive(false);
                m_Warren2G.gameObject.SetActive(true);

                m_Dwelling1Up.gameObject.SetActive(false);
            }
            else
            {
                m_WarrenG.gameObject.SetActive(true);
                m_Warren2G.gameObject.SetActive(false);

                m_Dwelling1.gameObject.SetActive(false);
            }
        }
        else
        {
            m_WarrenG.gameObject.SetActive(false);
            m_Warren2G.gameObject.SetActive(false);
        }

        if (a_Data.MageGuild5 && a_Data.FactionBuilding1)
        {
            m_ManaVortexMageGuild5.gameObject.SetActive(true);
            m_FactionBuilding1.gameObject.SetActive(false);
        }
        else
        {
            m_ManaVortexMageGuild5.gameObject.SetActive(false);
        }
    }
}
