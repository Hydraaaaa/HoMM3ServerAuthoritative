using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuildings : TownBuildings
{
    [Space]

    [SerializeField] Building m_GriffinG;
    [SerializeField] Building m_Griffin2G;
    [SerializeField] Building m_ShipyardMoat;
    [SerializeField] Building m_ShipyardMoatShip;

    public override void SetBuildings(BuildingData a_Data)
    {
        base.SetBuildings(a_Data);

        if (a_Data.Dwelling3Growth)
        {
            if (a_Data.Dwelling3Up)
            {
                m_GriffinG.gameObject.SetActive(false);
                m_Griffin2G.gameObject.SetActive(true);

                m_Dwelling3Up.gameObject.SetActive(false);
            }
            else
            {
                m_GriffinG.gameObject.SetActive(true);
                m_Griffin2G.gameObject.SetActive(false);

                m_Dwelling3.gameObject.SetActive(false);
            }
        }
        else
        {
            m_GriffinG.gameObject.SetActive(false);
            m_Griffin2G.gameObject.SetActive(false);
        }

        // Brotherhood of the Sword
        if (a_Data.FactionBuilding2)
        {
            m_Tavern.gameObject.SetActive(false);
        }

        if (a_Data.Shipyard)
        {
            if (a_Data.Citadel || a_Data.Castle)
            {
                m_Shipyard.gameObject.SetActive(false);
                m_ShipyardMoat.gameObject.SetActive(true);
            }
            else
            {
                m_Shipyard.gameObject.SetActive(true);
                m_ShipyardMoat.gameObject.SetActive(false);
            }
        }
        else
        {
            m_Shipyard.gameObject.SetActive(false);
            m_ShipyardMoat.gameObject.SetActive(false);
        }

        m_ShipyardMoatShip.gameObject.SetActive(false);
    }
}
