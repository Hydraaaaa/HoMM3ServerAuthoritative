using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuildings : TownBuildings
{
    [Space]

    [SerializeField] Building m_GriffinG;
    [SerializeField] Building m_Griffin2G;
    [SerializeField] Building m_Brotherhood;
    [SerializeField] Building m_Stables;
    [SerializeField] Building m_Shipyard;
    [SerializeField] Building m_ShipyardShip;
    [SerializeField] Building m_ShipyardMoat;
    [SerializeField] Building m_ShipyardMoatShip;
    [SerializeField] Building m_Lighthouse;

    public override void SetBuildings(List<byte> a_Bytes)
    {
        base.SetBuildings(a_Bytes);

        if ((a_Bytes[3] & 64) == 64)
        {
            if ((a_Bytes[3] & 32) == 32)
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

        if ((a_Bytes[2] & 8) == 8)
        {
            m_Tavern.gameObject.SetActive(false);
            m_Brotherhood.gameObject.SetActive(true);
        }
        else
        {
            m_Brotherhood.gameObject.SetActive(false);
        }

        m_Stables.gameObject.SetActive((a_Bytes[2] & 16) == 16);

        m_ShipyardShip.gameObject.SetActive(false);
        m_ShipyardMoatShip.gameObject.SetActive(false);

        if ((a_Bytes[2] & 1) == 1)
        {
            if ((a_Bytes[0] & 16) == 16 ||
                (a_Bytes[0] & 8) == 8)
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

        m_Lighthouse.gameObject.SetActive((a_Bytes[2] & 4) == 4);
    }
}
